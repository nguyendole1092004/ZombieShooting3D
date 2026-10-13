# Script API Reference - Zombie Shooting Game

Tài liệu tham khảo nhanh các scripts trong game.

---

## 📁 Player Scripts

### PlayerMovement.cs
**Chức năng:** Điều khiển di chuyển của player (WASD, jump, sprint)

**Public Variables:**
```csharp
float walkSpeed = 5f;           // Tốc độ đi bộ
float sprintSpeed = 8f;         // Tốc độ chạy (giữ Shift)
float jumpForce = 5f;           // Lực nhảy
float gravity = -9.81f;         // Trọng lực
Transform groundCheck;          // Điểm check chạm đất
float groundDistance = 0.4f;    // Bán kính check ground
LayerMask groundMask;           // Layer của mặt đất
```

**Cách dùng:**
1. Attach vào Player GameObject
2. Cần có CharacterController component
3. Tạo Empty GameObject "GroundCheck" ở dưới chân player
4. Set layer Ground cho terrain/floor

---

### PlayerHealth.cs
**Chức năng:** Quản lý máu của player, tự động hồi máu

**Public Variables:**
```csharp
int maxHealth = 100;            // Máu tối đa
bool canRegenerate = true;      // Có tự hồi máu không
float regenDelay = 5f;          // Delay trước khi hồi máu
int regenAmount = 5;            // Lượng máu hồi mỗi lần
float regenRate = 1f;           // Tần suất hồi máu (giây)
```

**Public Methods:**
```csharp
void TakeDamage(int damage)     // Nhận sát thương
void Heal(int amount)           // Hồi máu
bool IsDead()                   // Kiểm tra đã chết chưa
```

**Cách dùng:**
1. Attach vào Player GameObject
2. Tự động update UI qua UIManager
3. Gọi từ ZombieAI khi zombie attack

---

### MouseLook.cs
**Chức năng:** Điều khiển camera FPS, nhìn xoay bằng chuột

**Public Variables:**
```csharp
float mouseSensitivity = 100f;  // Độ nhạy chuột
Transform playerBody;           // Reference đến Player transform
float topClamp = 90f;           // Giới hạn nhìn lên
float bottomClamp = -90f;       // Giới hạn nhìn xuống
```

**Cách dùng:**
1. Attach vào Camera (con của Player)
2. Kéo Player transform vào field playerBody
3. Tự động lock cursor

---

## 🔫 Weapon Scripts

### Gun.cs
**Chức năng:** Xử lý bắn súng, reload, damage

**Public Variables:**
```csharp
string gunName = "Pistol";      // Tên súng
int damage = 25;                // Damage mỗi phát
float fireRate = 0.2f;          // Thời gian giữa các phát (giây)
int maxAmmo = 30;               // Đạn tối đa
float reloadTime = 1.5f;        // Thời gian reload
float range = 100f;             // Tầm bắn
float recoilAmount = 2f;        // Độ giật súng

// Effects
ParticleSystem muzzleFlash;     // Hiệu ứng bắn
GameObject impactEffect;        // Hiệu ứng đạn trúng
AudioClip shootSound;           // Âm thanh bắn
AudioClip reloadSound;          // Âm thanh reload
```

**Public Methods:**
```csharp
bool IsReloading()              // Đang reload hay không
```

**Cách dùng:**
1. Attach vào weapon model trong WeaponHolder
2. Cần có AudioSource component
3. Assign các effects (optional)
4. Tự động bắn khi nhấn chuột trái
5. Auto reload khi hết đạn, hoặc nhấn R

---

### WeaponSwitcher.cs
**Chức năng:** Đổi vũ khí (số phím 1-2-3 hoặc scroll chuột)

**Public Variables:**
```csharp
Gun[] weapons;                  // Array chứa tất cả vũ khí
```

**Public Methods:**
```csharp
Gun GetCurrentWeapon()          // Lấy vũ khí hiện tại
```

**Cách dùng:**
1. Attach vào WeaponHolder (con của Camera)
2. Add tất cả vũ khí vào WeaponHolder
3. Kéo các Gun vào array weapons[]
4. Chỉ weapon đầu tiên sẽ active

**Controls:**
- Phím 1, 2, 3: Chuyển vũ khí tương ứng
- Scroll chuột lên/xuống: Chuyển vũ khí

---

## 🧟 Enemy Scripts

### ZombieAI.cs
**Chức năng:** AI zombie - tìm, đuổi, tấn công player

**Public Variables:**
```csharp
float detectionRange = 15f;     // Phạm vi phát hiện player
float attackRange = 2f;         // Phạm vi tấn công
float walkSpeed = 2f;           // Tốc độ đi
float chaseSpeed = 3.5f;        // Tốc độ đuổi
int attackDamage = 10;          // Damage mỗi đòn
float attackRate = 1f;          // Tấn công mỗi giây
```

**States:**
- Idle: Đứng yên
- Patrol: Tuần tra (chưa implement)
- Chase: Đuổi theo player
- Attack: Tấn công player

**Cách dùng:**
1. Attach vào Zombie prefab
2. Cần có NavMeshAgent component
3. Cần có Animator (optional, cho animation)
4. Player phải có tag "Player"
5. Cần bake NavMesh cho scene

---

### ZombieHealth.cs
**Chức năng:** Quản lý máu zombie, thưởng tiền khi chết

**Public Variables:**
```csharp
int maxHealth = 100;            // Máu tối đa
int moneyReward = 50;           // Tiền thưởng khi chết
GameObject deathEffect;         // Hiệu ứng chết
AudioClip deathSound;           // Âm thanh chết
```

**Public Methods:**
```csharp
void TakeDamage(int damage)     // Nhận sát thương
bool IsDead()                   // Kiểm tra đã chết chưa
```

**Cách dùng:**
1. Attach vào Zombie prefab
2. Tự động gọi GameManager.AddMoney() khi chết
3. Tự động destroy sau 3 giây

---

### ZombieSpawner.cs
**Chức năng:** Spawn zombie theo wave hoặc continuous

**Public Variables:**
```csharp
GameObject[] zombiePrefabs;     // Các loại zombie
Transform[] spawnPoints;        // Điểm spawn
int maxZombies = 10;            // Số zombie tối đa cùng lúc
float spawnRate = 5f;           // Spawn mỗi 5 giây
int zombiesPerWave = 3;         // Số zombie mỗi wave
bool enableWaves = true;        // Chế độ wave
float timeBetweenWaves = 10f;   // Nghỉ giữa các wave
```

**Cách dùng:**
1. Tạo Empty GameObject "ZombieSpawner"
2. Tạo nhiều Empty GameObject con làm spawn points
3. Attach script vào parent
4. Kéo zombie prefabs vào array
5. Kéo spawn points vào array

**2 Modes:**
- Continuous: Spawn liên tục khi < maxZombies
- Wave-based: Spawn theo đợt, tăng dần độ khó

---

## 🎮 Manager Scripts

### GameManager.cs
**Chức năng:** Quản lý game state, economy, pause/resume

**Public Variables:**
```csharp
bool isGameActive = true;       // Game đang chạy
bool isPaused = false;          // Game đang pause
int playerMoney = 0;            // Tiền của player
int totalKills = 0;             // Tổng số zombie đã giết
int currentWave = 1;            // Wave hiện tại
```

**Public Methods:**
```csharp
void AddMoney(int amount)       // Thêm tiền
bool SpendMoney(int amount)     // Trừ tiền (return false nếu không đủ)
void AddKill()                  // Tăng kill count
void UpdateWave(int wave)       // Update wave number
void PlayerDied()               // Gọi khi player chết
void PauseGame()                // Pause game
void ResumeGame()               // Resume game
void RestartGame()              // Restart scene
void QuitGame()                 // Thoát game
```

**Singleton Pattern:**
```csharp
GameManager.Instance.AddMoney(50);
```

**Cách dùng:**
1. Tạo Empty GameObject "GameManager"
2. Attach script
3. Chỉ có 1 instance trong scene
4. Gọi từ bất kỳ script nào qua Instance

---

### AudioManager.cs
**Chức năng:** Quản lý nhạc nền và sound effects

**Public Variables:**
```csharp
AudioSource musicSource;        // Source cho nhạc nền
AudioSource sfxSource;          // Source cho sound effects
AudioClip menuMusic;            // Nhạc menu
AudioClip gameplayMusic;        // Nhạc chơi game
AudioClip buttonClick;          // SFX button
AudioClip zombieGrowl;          // SFX zombie
AudioClip playerHurt;           // SFX player bị đánh
```

**Public Methods:**
```csharp
void PlayMusic(AudioClip clip)      // Play nhạc nền
void PlaySFX(AudioClip clip)        // Play sound effect
void SetMusicVolume(float volume)   // Set âm lượng nhạc
void SetSFXVolume(float volume)     // Set âm lượng SFX
```

**Singleton Pattern:**
```csharp
AudioManager.Instance.PlaySFX(shootSound);
```

**Cách dùng:**
1. Tạo Empty GameObject "AudioManager"
2. Add 2 AudioSource components
3. Attach script
4. Assign các AudioSource và AudioClip

---

## 🖥️ UI Scripts

### UIManager.cs
**Chức năng:** Quản lý tất cả UI elements

**Public Variables:**
```csharp
// HUD
TextMeshProUGUI moneyText;      // Hiển thị tiền
TextMeshProUGUI ammoText;       // Hiển thị đạn
TextMeshProUGUI killsText;      // Hiển thị kills
TextMeshProUGUI waveText;       // Hiển thị wave
Image healthBar;                // Thanh máu
GameObject crosshair;           // Crosshair

// Menus
GameObject pauseMenu;           // Pause menu
GameObject gameOverMenu;        // Game over menu
GameObject shopMenu;            // Shop menu
```

**Public Methods:**
```csharp
void UpdateMoney(int money)                     // Update money display
void UpdateKills(int kills)                     // Update kills display
void UpdateWave(int wave)                       // Update wave display
void UpdateHealthBar(int current, int max)      // Update health bar
void ShowPauseMenu()                            // Hiện pause menu
void HidePauseMenu()                            // Ẩn pause menu
void ShowGameOver()                             // Hiện game over
void ToggleShop()                               // Toggle shop on/off
```

**Cách dùng:**
1. Attach vào Canvas
2. Kéo tất cả UI elements vào fields
3. Tự động update khi gọi methods

---

### MainMenuController.cs
**Chức năng:** Tạo menu mở đầu và armory tương thích với thao tác chạm trên Android

- **PLAY:** Mở scene gameplay `testscene`
- **ARMORY:** Dùng credits đã lưu để mở khóa AK74 và Shotgun
- Credits và vũ khí đã mở khóa được lưu giữa các lần chạy; vũ khí đã mở khóa được thêm vào loadout khi vào gameplay.

Scene `MainMenu.unity` cần đứng đầu danh sách Scenes In Build; `testscene.unity` cũng phải được thêm vào Build Settings.

---

### ShopUI.cs
**Chức năng:** UI cho shop, xử lý button clicks

**Public Variables:**
```csharp
WeaponShop weaponShop;          // Reference shop
Button[] weaponButtons;         // Buttons mua vũ khí
Button ammoButton;              // Button mua đạn
Button healthButton;            // Button mua máu
Button closeButton;             // Button đóng shop
int ammoPack = 50;              // Giá ammo
int healthPack = 100;           // Giá health
```

**Cách dùng:**
1. Attach vào ShopMenu panel
2. Link WeaponShop object
3. Kéo các buttons vào arrays
4. Tự động setup onClick listeners

---

## 🛒 System Scripts

### WeaponShop.cs
**Chức năng:** Logic mua bán vũ khí, item

**Public Variables:**
```csharp
WeaponItem[] weaponsForSale;    // Array vũ khí bán
Transform weaponHolder;         // Vị trí attach weapon
```

**WeaponItem Class:**
```csharp
string weaponName;              // Tên vũ khí
GameObject weaponPrefab;        // Prefab vũ khí
int price;                      // Giá
bool isPurchased;               // Đã mua chưa
```

**Public Methods:**
```csharp
bool BuyWeapon(int index)           // Mua vũ khí
bool BuyAmmo(int amount, int price) // Mua đạn
bool BuyHealthPack(int heal, int price) // Mua máu
```

**Cách dùng:**
1. Tạo Empty GameObject "WeaponShop"
2. Attach script
3. Kéo WeaponHolder từ player vào
4. Setup array weaponsForSale:
   - Weapon Name: "AK47"
   - Weapon Prefab: Kéo prefab vào
   - Price: 500
   - Is Purchased: false

---

### ImpactEffect.cs
**Chức năng:** Tự động destroy particle effect sau thời gian

**Public Variables:**
```csharp
float lifetime = 2f;            // Thời gian tồn tại
```

**Cách dùng:**
1. Attach vào Impact Effect prefab
2. Tự động destroy sau lifetime giây

---

## 🎯 Ví dụ sử dụng

### Zombie gây damage cho player:
```csharp
// Trong ZombieAI.cs
PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
playerHealth.TakeDamage(attackDamage);
```

### Player bắn zombie:
```csharp
// Trong Gun.cs
ZombieHealth zombie = hit.transform.GetComponent<ZombieHealth>();
zombie.TakeDamage(damage);
```

### Thêm tiền khi giết zombie:
```csharp
// Trong ZombieHealth.cs
GameManager.Instance.AddMoney(moneyReward);
```

### Mua vũ khí:
```csharp
// Trong ShopUI.cs
weaponShop.BuyWeapon(0); // Mua weapon index 0
```

### Play sound effect:
```csharp
// Trong bất kỳ script nào
AudioManager.Instance.PlaySFX(zombieGrowlSound);
```

---

## 📌 Lưu ý quan trọng

### Tags cần thiết:
- **Player**: Gắn cho Player GameObject
- **Enemy**: Gắn cho tất cả Zombie prefabs
- **MainCamera**: Gắn cho Camera

### Layers cần thiết:
- **Player**: Layer 6
- **Enemy**: Layer 7
- **Ground**: Layer 8

### Dependencies giữa các scripts:
```
GameManager (singleton)
    ↓
UIManager → Gun → WeaponSwitcher
    ↓
ZombieHealth → ZombieAI
    ↓
PlayerHealth → PlayerMovement
```

### Thứ tự setup:
1. Player + Camera + MouseLook
2. Gun + WeaponSwitcher
3. PlayerHealth + PlayerMovement
4. Zombie + ZombieAI + ZombieHealth
5. ZombieSpawner
6. GameManager
7. UIManager
8. WeaponShop + ShopUI

---

## 🔧 Debugging Tips

### Zombie không đuổi player:
- Check tag "Player"
- Check NavMesh đã bake
- Check detectionRange đủ lớn

### Không bắn được:
- Check Camera tag "MainCamera"
- Check Gun có AudioSource
- Check ammo > 0

### UI không update:
- Check UIManager references
- Check FindObjectOfType() có tìm thấy không

### Crash khi build Android:
- Check IL2CPP backend
- Check API Level >= 24
- Remove unused assets

---

**Last Updated:** 2026-10-12
**Unity Version:** 6.0 (6000.6.5f1)
