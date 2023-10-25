using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleScreen_BattleSkill {
    BULLET,
    GUN,
    SHIELD
}

public class BattleScreen_BattleController : CollectionScreen_MonoBehaviour {
    [field: Header("References")]
    [field: SerializeField] public Image PlayerCurrSkillImage { get; private set; }
    [field: SerializeField] public Image EnemyCurrSkillImage { get; private set; }
    [field: SerializeField] public Button BulletButton { get; private set; }
    [field: SerializeField] public Button GunButton { get; private set; }
    [field: SerializeField] public Button ShieldButton { get; private set; }
    [field: SerializeField] public TextMeshProUGUI PlayerBulletCountText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI EnemyBulletCountText { get; private set; }
    [field: SerializeField] public GameObject WinUIGameObj { get; private set; }
    [field: SerializeField] public GameObject LoseUIGameObj { get; private set; }
    [field: SerializeField] public GameObject DrawUIGameObj { get; private set; }
    [field: Header("Data")]
    [field: SerializeField] public Sprite BulletSprite { get; private set; }
    [field: SerializeField] public Sprite GunSprite { get; private set; }
    [field: SerializeField] public Sprite ShieldSprite { get; private set; }
    [field: Header("Runtime")]
    [field: SerializeField] public int PlayerBulletCount { get; private set; }
    [field: SerializeField] public BattleScreen_BattleSkill PlayerCurrSkill { get; private set; }
    [field: SerializeField] public int Seed { get; private set; }
    [field: SerializeField] public int EnemyBulletCount { get; private set; }
    [field: SerializeField] public BattleScreen_BattleSkill EnemyCurrSkill { get; private set; }
    private System.Random m_EnemyRng;
    private void OnEnable() {
        BulletButton.onClick.AddListener(OnBulletBtnClicked);
        GunButton.onClick.AddListener(OnGunBtnClicked);
        ShieldButton.onClick.AddListener(OnShieldBtnClicked);
    }
    private void OnDisable() {
        BulletButton.onClick.RemoveListener(OnBulletBtnClicked);
        GunButton.onClick.RemoveListener(OnGunBtnClicked);
        ShieldButton.onClick.RemoveListener(OnShieldBtnClicked);
    }
    public void OnBulletBtnClicked() {
        if (!TryUseSkill(BattleScreen_BattleSkill.BULLET)) {
            return;
        }
        DoEnemySkill();
        DoBattleLogic();
    }
    public void OnGunBtnClicked() {
        if (!TryUseSkill(BattleScreen_BattleSkill.GUN)) {
            return;
        }
        DoEnemySkill();
        DoBattleLogic();
    }
    public void OnShieldBtnClicked() {
        if (!TryUseSkill(BattleScreen_BattleSkill.SHIELD)) {
            return;
        }
        DoEnemySkill();
        DoBattleLogic();
    }
    public void RestartGame() {
        m_EnemyRng = new System.Random();
        PlayerBulletCount = 0;
        EnemyBulletCount = 0;
        PlayerCurrSkillImage.sprite = null;
        PlayerCurrSkillImage.DOFade(0f, 0f);
        EnemyCurrSkillImage.sprite = null;
        EnemyCurrSkillImage.DOFade(0f, 0f);
        PlayerBulletCountText.text = PlayerBulletCount.ToString();
        EnemyBulletCountText.text = EnemyBulletCount.ToString();
    }
    public bool TryUseSkill(BattleScreen_BattleSkill skill) {
        switch (skill) {
            case BattleScreen_BattleSkill.BULLET:
                PlayerBulletCount++;
                PlayerCurrSkill = skill;
                return true;
            case BattleScreen_BattleSkill.GUN:
                if (PlayerBulletCount <= 0) {
                    return false;
                }
                PlayerBulletCount--;
                PlayerCurrSkill = skill;
                return true;
            case BattleScreen_BattleSkill.SHIELD:
                PlayerCurrSkill = skill;
                return true;
        }
        return false;
    }
    public void DoEnemySkill() {
        // Randomize until valid
        BattleScreen_BattleSkill skill = (BattleScreen_BattleSkill)m_EnemyRng.Next(0, 3);
        while (EnemyBulletCount <= 0 && skill == BattleScreen_BattleSkill.GUN
            || PlayerBulletCount <= 1 && skill == BattleScreen_BattleSkill.SHIELD) {
            skill = (BattleScreen_BattleSkill)m_EnemyRng.Next(0, 3);
        }
        switch (skill) {
            case BattleScreen_BattleSkill.BULLET:
                EnemyBulletCount++;
                EnemyCurrSkill = skill;
                break;
            case BattleScreen_BattleSkill.GUN:
                EnemyBulletCount--;
                EnemyCurrSkill = skill;
                break;
            case BattleScreen_BattleSkill.SHIELD:
                EnemyCurrSkill = skill;
                break;
        }
    }
    public void DoBattleLogic() {
        // Update UI
        PlayerCurrSkillImage.sprite = GetSkillSprite(PlayerCurrSkill);
        PlayerCurrSkillImage.DOFade(1f, 0f);
        PlayerCurrSkillImage.SetNativeSize();
        PlayerBulletCountText.text = PlayerBulletCount.ToString();
        EnemyCurrSkillImage.sprite = GetSkillSprite(EnemyCurrSkill);
        EnemyCurrSkillImage.DOFade(1f, 0f);
        EnemyCurrSkillImage.SetNativeSize();
        EnemyBulletCountText.text = EnemyBulletCount.ToString();
        // Game logic
        if (PlayerCurrSkill == BattleScreen_BattleSkill.GUN && EnemyCurrSkill == BattleScreen_BattleSkill.GUN) {
            // Draw
            WinUIGameObj.SetActive(false);
            LoseUIGameObj.SetActive(false);
            DrawUIGameObj.SetActive(true);
            EconomyManager.AddCoin(2);
        } else if (PlayerCurrSkill == BattleScreen_BattleSkill.GUN && EnemyCurrSkill != BattleScreen_BattleSkill.SHIELD) {
            // Player win
            WinUIGameObj.SetActive(true);
            LoseUIGameObj.SetActive(false);
            DrawUIGameObj.SetActive(false);
            EconomyManager.AddCoin(5);
        } else if (EnemyCurrSkill == BattleScreen_BattleSkill.GUN && PlayerCurrSkill != BattleScreen_BattleSkill.SHIELD) {
            // Enemy win
            WinUIGameObj.SetActive(false);
            LoseUIGameObj.SetActive(true);
            DrawUIGameObj.SetActive(false);
            EconomyManager.AddCoin(1);
        }
    }
    public Sprite GetSkillSprite(BattleScreen_BattleSkill skill) {
        switch (skill) {
            case BattleScreen_BattleSkill.BULLET:
                return BulletSprite;
            case BattleScreen_BattleSkill.GUN:
                return GunSprite;
            case BattleScreen_BattleSkill.SHIELD:
                return ShieldSprite;
        }
        return null;
    }
}
