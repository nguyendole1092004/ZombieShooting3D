# 🎯 Game Architecture & Flow - Zombie Shooting

Visual diagrams và flow charts để hiểu rõ cấu trúc game.

---

## 🏗️ Game Object Hierarchy

```
Scene: testscene (gameplay; MainMenu is the startup scene)
│
├── 🗺️ Environment
│   ├── Plane (Ground)                [Layer: Ground]
│   ├── Directional Light
│   └── Props (từ Damaged Models pack1)
│
├── 👤 Player                         [Tag: Player, Layer: Player]
│   ├── CharacterController
│   ├── PlayerMovement.cs
│   ├── PlayerHealth.cs
│   │
│   ├── 📷 MainCamera                 [Tag: MainCamera]
│   │   ├── Camera
│   │   ├── MouseLook.cs
│   │   │
│   │   └── 🔫 WeaponHolder
│   │       ├── WeaponSwitcher.cs
│   │       │
│   │       ├── M1911 (Pistol)
│   │       │   ├── Gun.cs
│   │       │   └── AudioSource
│   │       │
│   │       ├── AK74 (mua từ shop)
│   │       └── Shotgun (mua từ shop)
│   │
│   └── ⚫ GroundCheck (Empty)
│
├── 🧟 ZombieSpawner
│   ├── ZombieSpawner.cs
│   │
│   ├── Spawn động trên NavMesh quanh Player (18–30 units)
│   └── Waves 1–10 (20 zombie/wave; tốc độ tăng theo wave)
│
├── 🗺️ Map
│   └── 4 GroundTile (ghép vuông 2×2, NavMeshSurface)
│
├── 🎮 Managers
│   ├── GameManager
│   │   └── GameManager.cs          [Singleton]
│   │
│   └── AudioManager
│       ├── AudioManager.cs         [Singleton]
│       ├── AudioSource (Music)
│       └── AudioSource (SFX)
│
├── 🛒 Shop
│   └── WeaponShop
│       └── WeaponShop.cs
│
└── 🖥️ Canvas (UI)
    ├── UIManager.cs
    ├── EventSystem
    │
    ├── HUD (Always visible)
    │   ├── HealthBar
    │   │   ├── HealthBarBG (Image)
    │   │   └── HealthBarFill (Image - Filled)
    │   ├── MoneyText (TMP)
    │   ├── AmmoText (TMP)
    │   ├── KillsText (TMP)
    │   ├── WaveText (wave hiện tại / 10 + survival timer)
    │   └── Crosshair (Image)
    │
    ├── PauseMenu (Hidden)
    │   ├── Panel
    │   ├── Title Text
    │   ├── Resume Button
    │   ├── Restart Button
    │   └── Quit Button
    │
    ├── GameOverMenu (Hidden)
    │   ├── Panel
    │   ├── Game Over Text
    │   ├── Final Kills + Survival Time
    │   ├── Final Wave / kết quả thắng
    │   ├── Restart Button
    │   └── Quit Button
    │
    └── ShopMenu (Hidden)
        ├── ShopUI.cs
        ├── Panel
        ├── Shop Title
        ├── Weapon1 Button
        ├── Weapon2 Button
        ├── Ammo Button
        ├── Health Button
        └── Close Button
```

---

## 🔄 Game Flow

```
┌─────────────────────────────────────────────────────────────┐
│                    GAME START                               │
└────────────────────┬────────────────────────────────────────┘
                     │
                     ▼
        ┌────────────────────────┐
        │  GameManager.Start()   │
        │  - Init money: $500    │
        │  - Init kills: 0       │
        │  - Lock cursor         │
        └────────┬───────────────┘
                 │
                 ▼
        ┌────────────────────────┐
        │ ZombieSpawner.Start()  │
        │ - Start Wave 1/10      │
        │ - Spawn 20 zombies     │
        └────────┬───────────────┘
                 │
                 ▼
     ┌───────────────────────────────┐
     │       GAMEPLAY LOOP           │
     │                               │
     │  Player:                      │
     │   - Move (WASD)              │
     │   - Look (Mouse)             │
     │   - Shoot (Click)            │
     │   - Reload (R)               │
     │                               │
     │  Zombie:                      │
     │   - Detect player (15m)      │
     │   - Chase player             │
     │   - Attack player (2m)       │
     │                               │
     └───────┬───────────────────────┘
             │
             ├─────────────┐
             │             │
             ▼             ▼
    ┌────────────┐  ┌─────────────┐
    │ Zombie Dies│  │ Player Hit  │
    │            │  │             │
    │ +$50       │  │ -10 HP      │
    │ +1 Kill    │  │ Update UI   │
    └─────┬──────┘  └──────┬──────┘
          │                │
          ▼                │
   ┌──────────────┐        │
   │ All zombies  │        │
   │ killed?      │        │
   └───┬──────────┘        │
       │                   │
       │ YES               │
       ▼                   │
   ┌──────────────┐        │
   │ Wave Complete│        │
   │ Wait 8s      │        │
   │ 20 zombies   │        │
   │ +speed/wave  │        │
   └───┬──────────┘        │
       │                   │
       │                   ▼
       │            ┌──────────────┐
       │            │ Player HP≤0? │
       │            └──────┬───────┘
       │                   │
       │                   │ YES
       │                   ▼
       │            ┌──────────────┐
       │            │  GAME OVER   │
       │            │              │
       │            │ Show stats   │
       │            │ - Total kills│
       │            │ - Wave reach │
       │            │ - Survival   │
       │            │              │
       │            │ [Restart]    │
       │            │ [Quit]       │
       │            └──────────────┘
       │
       └─────────► Wave 10 cleared → VICTORY
```

---

## 🎯 Script Communication Flow

```
                    GameManager (Singleton)
                           │
           ┌───────────────┼───────────────┐
           │               │               │
           ▼               ▼               ▼
      UIManager      PlayerHealth    ZombieHealth
           │               │               │
           │               │               │
     Updates UI      Takes damage    Rewards money
                           │               │
                           │               ▼
                           │         GameManager
                           │         .AddMoney()
                           │         .AddKill()
                           │
                           ▼
                    ZombieAI attacks
                           │
                           └──► PlayerHealth
                                .TakeDamage()
                                      │
                                      ▼
                                 UIManager
                                 .UpdateHealthBar()


       Gun.Shoot()
           │
           ├─ Raycast ──► Hit Zombie?
           │                   │
           │                   ▼
           │              ZombieHealth
           │              .TakeDamage()
           │                   │
           │                   ▼
           │              Zombie dies
           │                   │
           │                   ▼
           │              GameManager
           │              .AddMoney(50)
           │              .AddKill()
           │
           └─ Update ──► UIManager
                         .UpdateAmmo()
```

---

## 🧩 Script Dependencies

```
┌─────────────────────────────────────────────────────────────┐
│                    CORE SYSTEMS                             │
│  (Luôn cần có trong scene)                                 │
└─────────────────────────────────────────────────────────────┘

  GameManager.cs ◄────── Tất cả scripts khác
  (Singleton - quản lý game state)
       │
       ├─► UIManager.cs
       │   (Update displays)
       │
       └─► AudioManager.cs
           (Play sounds)


┌─────────────────────────────────────────────────────────────┐
│                   PLAYER SYSTEM                             │
└─────────────────────────────────────────────────────────────┘

  PlayerMovement.cs
       │
       ├─ Requires: CharacterController
       └─ Needs: GroundCheck transform


  PlayerHealth.cs
       │
       └─► UIManager.UpdateHealthBar()


  MouseLook.cs
       │
       └─ Needs: Player transform reference


  WeaponSwitcher.cs
       │
       └─ Contains: Gun[] array


  Gun.cs
       │
       ├─ Requires: AudioSource
       ├─► Raycast ──► ZombieHealth.TakeDamage()
       └─► UIManager (for ammo display)


┌─────────────────────────────────────────────────────────────┐
│                   ENEMY SYSTEM                              │
└─────────────────────────────────────────────────────────────┘

  ZombieAI.cs
       │
       ├─ Requires: NavMeshAgent
       ├─ Finds: Player by tag "Player"
       └─► PlayerHealth.TakeDamage()


  ZombieHealth.cs
       │
       ├─► GameManager.AddMoney()
       ├─► GameManager.AddKill()
       └─ References: ZombieAI


  ZombieSpawner.cs
       │
       ├─ Needs: Zombie prefabs[]
       ├─ Needs: Spawn points[]
       └─► GameManager.UpdateWave()


┌─────────────────────────────────────────────────────────────┐
│                    SHOP SYSTEM                              │
└─────────────────────────────────────────────────────────────┘

  WeaponShop.cs
       │
       ├─ Needs: WeaponHolder transform
       ├─► GameManager.SpendMoney()
       └─► WeaponSwitcher (add weapons)


  ShopUI.cs
       │
       ├─ References: WeaponShop
       ├─ References: Button[] array
       └─► UIManager.ToggleShop()
```

---

## 📊 Data Flow - Money System

```
Zombie chết
    │
    ▼
ZombieHealth.Die()
    │
    ├─ moneyReward = 50
    │
    ▼
GameManager.AddMoney(50)
    │
    ├─ playerMoney += 50
    │
    ▼
UIManager.UpdateMoney(playerMoney)
    │
    ▼
MoneyText.text = "$550"


Player mua weapon
    │
    ▼
ShopUI.OnWeaponButtonClicked()
    │
    ▼
WeaponShop.BuyWeapon(index)
    │
    ├─ Check: playerMoney >= price?
    │     │
    │     ▼ YES
    │
    ▼
GameManager.SpendMoney(500)
    │
    ├─ playerMoney -= 500
    │
    ▼
UIManager.UpdateMoney(playerMoney)
    │
    ▼
MoneyText.text = "$50"
    │
    ▼
Instantiate weapon vào WeaponHolder
    │
    ▼
WeaponSwitcher.weapons array updated
```

---

## 🎮 Input Handling Flow

```
User Input
    │
    ├─ WASD ──────────► PlayerMovement
    │                   .Move()
    │
    ├─ Mouse Move ────► MouseLook
    │                   .Look()
    │
    ├─ Left Click ────► Gun
    │                   .Shoot()
    │
    ├─ R ─────────────► Gun
    │                   .StartReload()
    │
    ├─ Space ─────────► PlayerMovement
    │                   .Jump()
    │
    ├─ 1/2/3 ─────────► WeaponSwitcher
    │                   .SwitchWeapon(index)
    │
    ├─ Scroll ────────► WeaponSwitcher
    │                   .SwitchWeapon()
    │
    └─ ESC ───────────► GameManager
                        .PauseGame()
                             │
                             ▼
                        UIManager
                        .ShowPauseMenu()
```

---

## 🧟 Zombie AI State Machine

```
                    START
                      │
                      ▼
                ┌──────────┐
                │   IDLE   │
                └─────┬────┘
                      │
                      │ Player trong
                      │ detection range?
                      │
                      ▼ YES
                ┌──────────┐
                │  CHASE   │◄─────┐
                └─────┬────┘      │
                      │           │
          ┌───────────┼───────────┤
          │           │           │
     Player xa?   Player gần?   Đang đuổi
          │           │           │
          ▼           ▼           │
    ┌──────────┐ ┌──────────┐    │
    │   IDLE   │ │  ATTACK  │────┘
    └──────────┘ └─────┬────┘
                       │
                       │ Player chạy xa?
                       │
                       ▼
                 ┌──────────┐
                 │  CHASE   │
                 └──────────┘
```

**States:**
- **IDLE**: Đứng yên, check phạm vi
- **CHASE**: Đuổi theo player (NavMesh)
- **ATTACK**: Tấn công player (1 hit/giây)

---

## 📦 Prefab Structure

### Zombie Prefab:
```
Zombie1
├── Model (Mesh/Skinned Mesh Renderer)
├── Animator (optional)
├── Capsule Collider
├── NavMeshAgent
├── ZombieAI.cs
└── ZombieHealth.cs

Tag: Enemy
Layer: Enemy
```

### Weapon Prefab:
```
M1911
├── Model (Mesh)
├── MuzzleFlash (Particle System)
├── Gun.cs
└── Audio Source

Parent: WeaponHolder
Position: (0.3, -0.3, 0.5) - điều chỉnh
```

---

## 🎨 UI Layout

```
┌──────────────────────────────────────────────────────────────┐
│ [❤️ ████████░░] 80/100        WAVE: 3        $1250 💰      │  ← Top
│                                                              │
│                                                              │
│                            ┼                                 │  ← Center
│                       (Crosshair)                            │  (Crosshair)
│                                                              │
│                                                              │
│ Kills: 47                                      🔫 15 / 30   │  ← Bottom
└──────────────────────────────────────────────────────────────┘

HUD Elements:
• Health Bar (top left)
• Wave Counter (top center)
• Money (top right)
• Crosshair (center)
• Kills (bottom left)
• Ammo (bottom right)
```

---

## 🔧 Setup Order (Critical!)

```
1. ✅ Create Layers & Tags FIRST
   │
   ▼
2. ✅ Setup Map + Bake NavMesh
   │
   ▼
3. ✅ Create Player
   ├─ Movement + Camera
   ├─ Health
   └─ Weapons
   │
   ▼
4. ✅ Create Zombie Prefab
   ├─ AI + Health
   └─ Test movement on NavMesh
   │
   ▼
5. ✅ Create Managers
   ├─ GameManager
   └─ AudioManager
   │
   ▼
6. ✅ Create UI
   ├─ HUD
   ├─ Menus
   └─ Link to UIManager
   │
   ▼
7. ✅ Create Spawner + Shop
   │
   ▼
8. ✅ Test Everything!
```

**Tại sao thứ tự này quan trọng?**
- Layers/Tags phải có trước khi assign
- NavMesh cần có map
- Player cần có trước khi zombie tìm target
- Managers cần có trước khi scripts khác gọi

---

## 💡 Performance Tips

### Object Pooling (Advanced):
Thay vì Instantiate/Destroy zombie liên tục:
```
Spawn 10 zombies 1 lần
  ↓
Khi zombie chết: Disable, không destroy
  ↓
Khi cần spawn: Enable zombie đã disable
  ↓
Tiết kiệm performance!
```

### LOD (Level of Detail):
- Zombie xa: Low poly model
- Zombie gần: High poly model

### Occlusion Culling:
- Không render objects bị che khuất
- Unity tự động xử lý

---

**Đọc file này cùng với SETUP_CHECKLIST.md để hiểu rõ toàn bộ cấu trúc game!** 🎮
