# 🧟 Zombie Shooting Game

**Game bắn zombie 3D góc nhìn thứ nhất (FPS) cho Android**

![Unity Version](https://img.shields.io/badge/Unity-6.0%20(6000.6.5f1)-black?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20PC-green)
![Status](https://img.shields.io/badge/Status-Complete-brightgreen)
![Scripts](https://img.shields.io/badge/Scripts-15%20files-blue)

---

## 📖 Giới thiệu

Game FPS bắn zombie đơn giản được phát triển bằng Unity 6, dành cho đồ án môn Game Development. Game có đầy đủ các tính năng cơ bản:

- 🎯 Bắn zombie với nhiều loại vũ khí
- 🏃 Di chuyển, nhảy, chạy nhanh
- 🧟 Zombie AI tự động đuổi theo và tấn công
- 💰 Hệ thống kiếm tiền và mua sắm
- 📊 Wave system với độ khó tăng dần
- ❤️ Health system với auto-regeneration
- 🎨 UI đầy đủ (HUD, shop, pause menu, game over)

---

## 🎮 Screenshots

*(Thêm screenshots sau khi setup xong trong Unity)*

---

## 🚀 Quick Start

### Main menu

The game opens in `Assets/Scenes/MainMenu.unity`. Tap **PLAY** to load the existing
`testscene` gameplay scene, or open **ARMORY** to spend saved credits on permanent
AK74 and Shotgun unlocks. Credits earned from zombie kills and bought weapons are
saved between runs.

### Yêu cầu
- Unity 6.0 (6000.6.5f1) hoặc mới hơn
- TextMeshPro package (built-in)
- Universal RP (built-in)

### Cài đặt

1. **Clone hoặc mở project**
   ```bash
   # Nếu từ git
   git clone <repository-url>
   
   # Hoặc mở trực tiếp trong Unity Hub
   ```

2. **Mở project trong Unity**
   - Unity Hub > Add > Chọn folder project
   - Chọn Unity 6.0 (6000.6.5f1)

3. **Tự dựng UI và cấu hình gameplay trong Unity**
   
   📋 **Đọc file này trước:** [`QUICK_START.md`](QUICK_START.md) để tự tạo Canvas, panel, nút và gán `MainMenuController` trong Inspector.
   
   Dùng checklist nếu muốn tự setup/chỉnh từng module gameplay:
   - Player và camera
   - Zombie AI
   - UI system
   - Game managers
   - Shop system

4. **Play và test**
   - Nhấn Play trong Unity Editor
   - Test tất cả features

---

## 📚 Documentation

| File | Mô tả |
|------|-------|
| **[QUICK_START.md](QUICK_START.md)** | 🚀 Tự tạo giao diện menu/shop trong Unity và gán references |
| **[SETUP_CHECKLIST.md](SETUP_CHECKLIST.md)** | ✅ Checklist setup từng module gameplay/menu |
| **[GAME_SETUP_GUIDE.md](GAME_SETUP_GUIDE.md)** | 📖 Hướng dẫn chi tiết setup và build |
| **[SCRIPT_REFERENCE.md](SCRIPT_REFERENCE.md)** | 📝 API documentation cho tất cả scripts |
| **[LAYERS_TAGS_GUIDE.md](LAYERS_TAGS_GUIDE.md)** | 🏷️ Hướng dẫn Layers & Tags trong Unity |
| **[GAME_ARCHITECTURE.md](GAME_ARCHITECTURE.md)** | 🏗️ Visual diagrams và flow charts |
| **[LEARNING_GUIDE.md](LEARNING_GUIDE.md)** | 🎓 Learning resources cho sinh viên |
| **[COMMON_ERRORS.md](COMMON_ERRORS.md)** | 🐛 Tổng hợp lỗi thường gặp & fixes |

---

## 🎯 Features

### ✅ Player System
- Di chuyển WASD với CharacterController
- Nhảy (Space) và chạy nhanh (Shift)
- Camera FPS với mouse look
- Health system với auto-regen
- Có thể chết và game over

### ✅ Weapon System
- Bắn súng với raycast
- Reload tự động hoặc thủ công (R)
- Đổi vũ khí (1-2-3 hoặc scroll chuột)
- Hỗ trợ nhiều loại vũ khí
- Ammo system

### ✅ Enemy System (Zombie AI)
- NavMesh AI với 4 states:
  - Idle: Đứng yên
  - Patrol: Tuần tra (có thể mở rộng)
  - Chase: Đuổi theo player
  - Attack: Tấn công player
- Detection range và attack range
- Gây damage cho player
- Drop money khi chết

### ✅ Game Systems
- **Wave System**: Zombie spawn theo đợt, tăng độ khó
- **Economy**: Kiếm tiền từ zombie, mua vũ khí/items
- **Shop**: Mua vũ khí, đạn, máu
- **Game Manager**: Quản lý game state, pause/resume
- **Audio Manager**: Nhạc nền và sound effects

### ✅ UI System
- HUD: Health bar, ammo, money, kills, wave
- Crosshair
- Pause menu (ESC)
- Game Over screen với stats
- Shop menu với buttons

---

## 🎮 Controls

| Phím | Chức năng |
|------|-----------|
| **WASD** | Di chuyển |
| **Mouse** | Nhìn xung quanh |
| **Left Click** | Bắn |
| **R** | Reload |
| **Space** | Nhảy |
| **Left Shift** | Chạy nhanh |
| **1, 2, 3** | Đổi vũ khí |
| **Mouse Scroll** | Đổi vũ khí |
| **ESC** | Pause menu |

---

## 📁 Project Structure

```
ZombieShooting/
├── Assets/
│   ├── Script/                     # ⭐ Tất cả scripts game
│   │   ├── Player/                 # 3 scripts
│   │   ├── Weapons/                # 2 scripts
│   │   ├── Enemy/                  # 3 scripts
│   │   ├── Managers/               # 2 scripts
│   │   ├── UI/                     # 3 scripts
│   │   └── Systems/                # 2 scripts
│   ├── Scenes/
│   │   ├── MainMenu.unity           # Startup menu and armory
│   │   └── testscene.unity          # Gameplay
│   ├── Zombie/                     # 3 zombie models
│   ├── Low Poly Weapons VOL.1/     # Weapon models
│   ├── Damaged Models pack1/       # Map props
│   └── Starter Assets/             # Unity starter pack
│
├── 📄 README.md                    # ← Bạn đang đọc file này
├── 📋 SETUP_CHECKLIST.md           # Checklist setup
├── 📖 GAME_SETUP_GUIDE.md          # Hướng dẫn chi tiết
├── 📝 SCRIPT_REFERENCE.md          # API docs
└── 📊 PROJECT_SUMMARY.md           # Tổng quan project
```

---

## 🔧 Scripts Overview

### Player Scripts (3)
- `PlayerMovement.cs` - Di chuyển, nhảy, sprint
- `PlayerHealth.cs` - Hệ thống máu với regen
- `MouseLook.cs` - Camera FPS control

### Weapon Scripts (2)
- `Gun.cs` - Bắn, reload, damage
- `WeaponSwitcher.cs` - Đổi vũ khí

### Enemy Scripts (3)
- `ZombieAI.cs` - AI với states machine
- `ZombieHealth.cs` - Máu zombie và rewards
- `ZombieSpawner.cs` - Spawn theo wave

### Manager Scripts (2)
- `GameManager.cs` - Game state, economy (Singleton)
- `AudioManager.cs` - Music và SFX (Singleton)

### UI Scripts (3)
- `UIManager.cs` - Quản lý tất cả UI
- `ShopUI.cs` - Shop button handlers
- `MainMenuController.cs` - Startup menu, armory, and saved weapon unlocks

### System Scripts (2)
- `WeaponShop.cs` - Logic mua bán
- `ImpactEffect.cs` - Bullet impact effects

**Xem chi tiết:** [SCRIPT_REFERENCE.md](SCRIPT_REFERENCE.md)

---

## 🏗️ Build cho Android

### 1. Switch Platform
```
File > Build Settings
Platform: Android
Switch Platform
```

### 2. Player Settings
```
Company Name: Tên của bạn
Product Name: Zombie Shooting
Package Name: com.yourname.zombieshooting
Minimum API Level: Android 7.0 (API 24)
Scripting Backend: IL2CPP
Target Architectures: ARM64 ✓
```

### 3. Build APK
```
File > Build Settings
Add Open Scenes
Build
```

**Chi tiết:** Xem [GAME_SETUP_GUIDE.md](GAME_SETUP_GUIDE.md) phần "Build cho Android"

---

## 🎓 Dành cho người mới học Unity

### Concepts được sử dụng
- ✅ GameObjects và Components
- ✅ Prefabs
- ✅ C# Classes và Methods
- ✅ Unity Lifecycle (Start, Update)
- ✅ Input System
- ✅ Raycast cho bắn súng
- ✅ NavMesh AI
- ✅ UI Canvas và TextMeshPro
- ✅ Audio System
- ✅ Singleton Pattern
- ✅ Layers và Tags

### Code Style
- Clean và có comment tiếng Việt
- Modular - Mỗi script 1 chức năng
- Public variables để customize trong Inspector
- Dễ đọc, dễ hiểu cho beginners

---

## 🐛 Troubleshooting

### ⚠️ Lỗi Input System (Unity 6)?
```
InvalidOperationException: You are trying to read Input using the UnityEngine.Input class
```
**FIX:** 
- Edit > Project Settings > Player > Other Settings
- Active Input Handling → **"Input Manager (Old)"**
- Restart Unity

**Xem chi tiết:** [FIX_INPUT_SYSTEM.md](FIX_INPUT_SYSTEM.md)

### Zombie không di chuyển?
- ✅ Kiểm tra đã Bake NavMesh chưa
- ✅ Player có tag "Player" chưa
- ✅ NavMeshAgent enabled chưa

### Không bắn được?
- ✅ Camera có tag "MainCamera" chưa
- ✅ Gun có AudioSource chưa
- ✅ Check ammo > 0

### UI không hiện?
- ✅ Canvas có EventSystem chưa
- ✅ UIManager đã link UI elements chưa
- ✅ TextMeshPro imported chưa

**Xem thêm:** [GAME_SETUP_GUIDE.md](GAME_SETUP_GUIDE.md) phần "Xử lý lỗi thường gặp"

---

## 💡 Mở rộng game

### Ideas để làm thêm:
- [ ] Thêm nhiều loại zombie (fast, tank, explode)
- [ ] Thêm boss zombie mỗi 5 waves
- [ ] Thêm nhiều loại vũ khí (sniper, shotgun, SMG)
- [ ] Thêm grenade và explosives
- [ ] Thêm power-ups (speed, damage boost, invincibility)
- [ ] Thêm multiple maps
- [ ] Main menu scene
- [ ] Options menu (volume, sensitivity)
- [ ] Leaderboard với PlayerPrefs
- [ ] Mobile touch controls
- [ ] Multiplayer (advanced)

---

## 📊 Game Stats

### Player
- Health: 100 HP
- Walk Speed: 5 m/s
- Sprint Speed: 8 m/s
- Starting Money: $500

### Zombie
- Health: 100 HP
- Speed: 2-3.5 m/s
- Damage: 10 HP/hit
- Reward: $50

### Weapons
- Pistol: 25 damage, 30 ammo
- AK74: (có thể mua, $500)
- Shotgun: (có thể mua, $1000)

---

## 🎨 Assets Credits

- **Zombie Models**: Assets/Zombie/
- **Weapon Models**: Low Poly Weapons VOL.1
- **Environment Props**: Damaged Models pack1
- **Player Controller Base**: Unity Starter Assets

---

## 📝 License

Dành cho mục đích học tập và đồ án.

---

## 👨‍💻 Development Info

**Ngôn ngữ:** C#  
**Engine:** Unity 6.0 (6000.6.5f1)  
**Platform:** Android (PC compatible)  
**Scripts:** 14 files  
**Lines of Code:** ~1500+  
**Development Time:** 1-2 giờ để code + 1-2 giờ để setup

---

## 📞 Support

Nếu gặp vấn đề:
1. Đọc [QUICK_START.md](QUICK_START.md) để tự dựng/gắn giao diện
2. Dùng [SETUP_CHECKLIST.md](SETUP_CHECKLIST.md) cho các module gameplay
3. Xem Troubleshooting trong [GAME_SETUP_GUIDE.md](GAME_SETUP_GUIDE.md)
4. Check Console log trong Unity
5. Google error message + "Unity"

---

## 🎉 Kết luận

**Game đã sẵn sàng để setup!**

👉 Bắt đầu với file: [**QUICK_START.md**](QUICK_START.md)

**Estimated setup time:** 1-2 giờ

**Good luck với đồ án! 🎮**

---

*Made with ❤️ using Unity 6*  
*October 12, 2026*
#   S h o o t i n g Z o m b i e 3 D  
 