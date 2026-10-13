# Zombie Shooting Game - Unity 3D FPS

Game bắn zombie góc nhìn thứ nhất đơn giản cho Android, làm bằng Unity 6.

## 📋 Tổng quan

Game FPS bắn zombie với các tính năng:
- ✅ Di chuyển, nhảy, sprint
- ✅ Bắn súng với nhiều loại vũ khí
- ✅ Zombie AI đuổi theo và tấn công
- ✅ Hệ thống máu cho player và zombie
- ✅ Economy system (kiếm tiền khi giết zombie)
- ✅ Shop mua vũ khí và item
- ✅ Wave system (zombie spawn theo đợt)
- ✅ UI đầy đủ (HUD, pause menu, game over)

## 🗂️ Cấu trúc Scripts

```
Assets/Script/
├── Player/
│   ├── PlayerMovement.cs      - Di chuyển player
│   ├── PlayerHealth.cs        - Máu player
│   └── MouseLook.cs           - Xoay camera FPS
├── Weapons/
│   ├── Gun.cs                 - Logic bắn súng
│   └── WeaponSwitcher.cs      - Đổi vũ khí
├── Enemy/
│   ├── ZombieAI.cs            - AI zombie
│   ├── ZombieHealth.cs        - Máu zombie
│   └── ZombieSpawner.cs       - Spawn zombie theo wave
├── Managers/
│   ├── GameManager.cs         - Quản lý game state
│   └── AudioManager.cs        - Quản lý âm thanh
├── UI/
│   ├── UIManager.cs           - Quản lý UI
│   ├── ShopUI.cs              - UI shop
│   └── MainMenuController.cs  - Menu mở đầu và armory
└── Systems/
    ├── WeaponShop.cs          - Logic mua bán
    └── ImpactEffect.cs        - Hiệu ứng đạn bắn
```

## 🎮 Hướng dẫn Setup trong Unity

> Menu chính và shop chỉ có logic trong `MainMenuController.cs`, `ShopUI.cs` và `WeaponShop.cs`; không script nào tự sinh giao diện. Tận dụng các UI object đã có trong scene, tự chỉnh/tạo UI thủ công trong Editor và gán references bằng Inspector. Xem hướng dẫn từng bước trong [QUICK_START.md](QUICK_START.md) hoặc [SETUP_CHECKLIST.md](SETUP_CHECKLIST.md).

### **BƯỚC 1: Setup Scene**

1. Mở scene gameplay `Assets/Scenes/testscene.unity`. `MainMenu.unity` là scene mở đầu trong Build Settings.
2. Scene `testscene` đã có map vuông 2×2 gồm bốn GroundTile; không tạo thêm Plane nếu đang dùng scene này. Nếu dựng scene mới:
   - GameObject > 3D Object > Plane (scale lên x10, x10, x10)
   - Add các prefab từ `Assets/Damaged Models pack1/Prefabs/` để trang trí map

### **BƯỚC 2: Setup Player**

1. **Tạo Player GameObject:**
   ```
   - Tạo Empty GameObject, đặt tên "Player"
   - Add Component: Character Controller
   - Add Component: PlayerMovement.cs
   - Add Component: PlayerHealth.cs
   - Tag: "Player"
   - Layer: "Player" (tạo layer mới nếu chưa có)
   ```

2. **Tạo Camera:**
   ```
   - Tạo Camera làm con của Player
   - Position: (0, 1.6, 0) - mắt player
   - Add Component: MouseLook.cs
   - Kéo Player transform vào field "Player Body"
   - Tag: "MainCamera"
   ```

3. **Tạo Ground Check (để check nhảy):**
   ```
   - Tạo Empty GameObject làm con của Player
   - Đặt tên "GroundCheck"
   - Position: (0, 0, 0) - dưới chân player
   - Kéo vào field "Ground Check" trong PlayerMovement
   ```

4. **Tạo Weapon Holder:**
   ```
   - Tạo Empty GameObject làm con của Camera
   - Đặt tên "WeaponHolder"
   - Position: (0.3, -0.3, 0.5) - góc phải dưới camera
   - Add Component: WeaponSwitcher.cs
   ```

5. **Add Weapon:**
   ```
   - Kéo prefab từ "Assets/Low Poly Weapons VOL.1/Prefabs/" vào WeaponHolder
   - Ví dụ: M1911.prefab (pistol)
   - Add Component: Gun.cs
   - Add Component: Audio Source
   - Điều chỉnh position/rotation để weapon nhìn đẹp
   - Kéo weapon này vào array "Weapons" trong WeaponSwitcher
   ```

### **BƯỚC 3: Setup Zombie**

1. **Tạo Zombie Prefab:**
   ```
   - Kéo prefab từ "Assets/Zombie/Prefabs/Zombie1.prefab" vào scene
   - Add Component: NavMeshAgent
   - Add Component: ZombieAI.cs
   - Add Component: ZombieHealth.cs
   - Add Component: Capsule Collider (nếu chưa có)
   - Tag: "Enemy"
   - Layer: "Enemy"
   
   Settings cho NavMeshAgent:
   - Speed: 2
   - Angular Speed: 120
   - Acceleration: 8
   - Stopping Distance: 2
   ```

2. **Bake NavMesh cho map:**
   - Trong `testscene`, chọn GameObject có component `NavMeshSurface` (đối tượng Plane).
   - Trong Inspector xác nhận `Collect Objects` bao gồm toàn bộ map; sau khi thêm map tiles hoặc props, bấm **Bake** trong component `NavMeshSurface`.
   - Kiểm tra bề mặt NavMesh phủ đủ bốn GroundTile và chỉ nằm trong phạm vi map trước khi chạy.

3. **Tạo Zombie Prefab:**
   ```
   - Kéo zombie từ scene vào folder "Assets/Zombie/Prefabs/"
   - Xóa zombie trong scene
   ```

### **BƯỚC 4: Setup Spawner**

1. **Thiết lập vùng spawn động:**
   ```
   - Tạo Empty GameObject tên "ZombieSpawner"
   - Position: (0, 0, 0)
   - Add Component: ZombieSpawner.cs
   ```
   Spawner tìm Player có Tag `Player` rồi chọn ngẫu nhiên vị trí NavMesh cách Player 18–30 units. SpawnPoint không cần tạo hoặc gán.

2. **Setup Spawner:**
   ```
   - Chọn ZombieSpawner object
   - Kéo các zombie prefab vào array "Zombie Prefabs"
   - Max Zombies: 20
   - Spawn Rate: 0.5 giây
   - Zombies Per Wave: 20
   - Minimum Spawn Distance: 18
   - Maximum Spawn Distance: 30
   - Nav Mesh Sample Distance: 5
   - Spawn Position Attempts: 30
   - Enable Waves: Check ✓
   - Time Between Waves: 8 giây
   - Walk Speed Increase Per Wave: 0.15
   - Chase Speed Increase Per Wave: 0.25
   ```
   Trận có đúng 10 wave, mỗi wave spawn 20 zombie. Wave kế tiếp chỉ bắt đầu sau khi toàn bộ zombie của wave hiện tại bị hạ; số lượng giữ nguyên nhưng tốc độ tăng dần. Hoàn thành wave 10 sẽ hiện Victory; pause/game over không tiếp tục spawn.

3. **Map vuông và NavMesh:**
   - Scene `testscene` ghép 4 Plane 100×100 thành map vuông 2×2 (xấp xỉ 200×200).
   - Sau khi thay đổi/kéo thêm props, chọn `Plane` có `NavMeshSurface`, Bake lại NavMesh để dữ liệu bao gồm đủ bốn GroundTile và các vật cản.
   - Player được giới hạn trong vùng X/Z từ -48 đến 148 (đặt trong `PlayerMovement > Map Min XZ / Map Max XZ`). Điều chỉnh nếu thay đổi kích thước hoặc vị trí map.
   - Zombie dùng NavMeshAgent nên chỉ đuổi trong vùng NavMesh đã bake. Bake lại sau khi chỉnh map và bảo đảm NavMesh không tràn khỏi mép nền.

### **BƯỚC 5: Setup Managers**

1. **Game Manager:**
   ```
   - Tạo Empty GameObject tên "GameManager"
   - Add Component: GameManager.cs
   ```

2. **Audio Manager:**
   ```
   - Tạo Empty GameObject tên "AudioManager"
   - Add Component: AudioManager.cs
   - Add 2 Audio Source components
   - Kéo vào fields Music Source và SFX Source
   ```

### **BƯỚC 6: Setup UI**

1. **Tạo Canvas:**
   ```
   - UI > Canvas
   - Canvas Scaler > UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920 x 1080
   ```

2. **Tạo HUD:**
   ```
   Trong Canvas, tạo:
   
   A. Health Bar (góc trái trên):
   - UI > Image (background)
   - UI > Image (fill) - màu đỏ
   - Image Type: Filled
   - Fill Method: Horizontal
   
   B. Money Text (góc phải trên):
   - UI > Text - TextMeshPro
   - Text: "$0"
   - Font Size: 36
   
   C. Ammo Text (góc phải dưới):
   - UI > Text - TextMeshPro
   - Text: "30 / 30"
   - Font Size: 30
   
   D. Kills Text (góc trái dưới):
   - UI > Text - TextMeshPro
   - Text: "Kills: 0"
   
   E. Wave Text (giữa trên):
   - UI > Text - TextMeshPro
   - Text: "WAVE: 1 / 10   TIME: 00:00"
   - UIManager tự cập nhật wave và thời gian sinh tồn
   
   F. Crosshair (chính giữa):
   - UI > Image
   - Sprite: Circle hoặc Cross
   - Color: White, Alpha: 0.5
   - Width x Height: 20 x 20
   ```

3. **Tạo Pause Menu:**
   ```
   - UI > Panel - đặt tên "PauseMenu"
   - Trong Panel:
     + UI > Text: "PAUSED"
     + UI > Button: "Resume"
     + UI > Button: "Restart"
     + UI > Button: "Quit"
   - Ẩn panel này (uncheck)
   ```

4. **Tạo Game Over Menu:**
   ```
   - UI > Panel - đặt tên "GameOverMenu"
   - Trong Panel:
     + UI > Text: "GAME OVER"
     + UI > Text: tổng kills và Survival Time
     + UI > Text: wave đã tới / 10
     + UI > Button: "Restart"
     + UI > Button: "Quit"
   - Ẩn panel này (uncheck)
   ```

5. **Tái sử dụng ShopMenu làm Armory:**
   - Không tạo ShopMenu/nút shop mới. Dùng object `ShopMenu`, `ShopUI`, các weapon buttons và catalog `WeaponShop` đã có trong `testscene`.
   - Tạo prefab `WeaponShopCatalog` từ component WeaponShop hiện có và prefab `Armory` từ ShopMenu hiện có; hướng dẫn chuyển prefab và gán references theo từng scene nằm trong [QUICK_START.md](QUICK_START.md).
   - Gán riêng `ShopUI > Weapon Shop` tới catalog prefab instance trong từng scene. Trong `testscene`, gán `Weapon Holder` tới WeaponHolder của Player; trong MainMenu để trống.
   - Armory dùng dữ liệu cũ (AK74 $500, Shotgun $1000); Ammo/Health vẫn mua được ở MainMenu và gameplay.
   - ShopUI đặt Ammo Pack Amount = 90, Ammo Pack Price = 50, Health Pack Price = 100; health pack hồi 50 HP khi sử dụng từ HUD.

6. **Setup UI Manager:**
   ```
   - Chọn Canvas
   - Add Component: UIManager.cs
   - Kéo tất cả UI elements vào các fields tương ứng
   ```

### **BƯỚC 7: Kết nối Shop System hiện có**

1. Không tạo GameObject `WeaponShop`, danh sách vũ khí hay `ShopUI` mới nếu các object hiện có trong `testscene` còn nguyên.
2. Kiểm tra catalog hiện có: AK74 ($500) và Shotgun ($1000), gồm prefab và chỉ số sát thương/tốc độ bắn/đạn.
3. Dùng chung prefab `WeaponShopCatalog` và ShopMenu prefab `Armory` ở MainMenu lẫn gameplay. Gán `ShopUI > Weapon Shop` tới catalog instance của từng scene.
4. Gameplay: gán `Weapon Holder` tới WeaponHolder của Player. MainMenu: để trống `Weapon Holder`; shop chỉ lưu unlock/credits, còn gameplay tự thêm vũ khí đã mở khóa vào loadout.
5. Kiểm tra `ShopUI > Weapon Buttons` có đúng thứ tự AK74 rồi Shotgun, cùng các references Ammo/Health/Close cũ. Chi tiết tạo prefab và bảng gán MainMenu xem [QUICK_START.md](QUICK_START.md).

### **BƯỚC 8: Tạo mobile controls & inventory HUD**

1. Trong Canvas gameplay, tạo `MobileControlsRoot`; bên trong đặt joystick dưới-trái, Fire/Reload/Switch/Heal/Jump dưới-phải, Shop/Pause và một vùng kéo ngắm không che nút.
2. Tạo hai TMP text riêng `ReserveAmmoText` và `HealthPacksText`; HUD hiện có `AmmoText` tiếp tục dùng cho số đạn trong băng.
3. Gắn `VirtualJoystick` vào joystick và nối với `PlayerMovement`; `MobileCombatControls` vào root và nối PlayerHealth, PlayerMovement, WeaponSwitcher cùng các nút; `MobileFireButton` vào nút Fire; `TouchLookArea` vào vùng aim và nối MouseLook.
4. Tạo Button `JumpButton`, đặt nhãn `JUMP`, gán Button vào `MobileCombatControls > Jump Button`. Script tự bắt sự kiện nút; không thêm OnClick thủ công. Player chỉ nhảy khi Ground Check đang chạm đất; thử bằng nút trên mobile và Space trong Editor.
5. Gán Mobile Controls Root và hai TMP inventory texts trong UIManager. Chi tiết hierarchy, anchors và bảng Inspector xem [QUICK_START.md](QUICK_START.md).
6. Player mới nhận 90 viên reserve ammo và 1 health pack. Reload dùng reserve; nút Heal tiêu thụ 1 pack và hồi 50 HP. Cả hai món mua được ở Armory trong menu/game.

### **BƯỚC 9: Setup Layers & Tags**

1. **Tags cần tạo:**
   ```
   - Player
   - Enemy
   - MainCamera
   ```

2. **Layers cần tạo:**
   ```
   - Player (Layer 6)
   - Enemy (Layer 7)
   - Ground (Layer 8)
   ```

3. **Layer Collision Matrix:**
   ```
   Edit > Project Settings > Physics
   Bỏ check:
   - Player vs Player
   - Enemy vs Enemy
   ```

### **BƯỚC 9: Input Settings**

```
Edit > Project Settings > Input Manager

Kiểm tra các input:
- Horizontal: A/D, Left/Right Arrow
- Vertical: W/S, Up/Down Arrow
- Fire1: Left Mouse Button
- Jump: Space
- Mouse X: Mouse Movement
- Mouse Y: Mouse Movement
```

### **BƯỚC 10: Build cho Android**

1. **Switch Platform:**
   ```
   File > Build Settings
   - Chọn Android
   - Click "Switch Platform"
   ```

2. **Player Settings:**
   ```
   Edit > Project Settings > Player
   
   - Company Name: Tên của bạn
   - Product Name: "Zombie Shooting"
   - Package Name: com.yourname.zombieshooting
   - Minimum API Level: Android 7.0 (API 24)
   - Target API Level: Automatic (highest installed)
   
   Other Settings:
   - Graphics APIs: OpenGLES3, OpenGLES2
   - Scripting Backend: IL2CPP
   - Target Architectures: ARM64 ✓
   ```

3. **Quality Settings:**
   ```
   Edit > Project Settings > Quality
   - Chọn preset "Medium" hoặc "Low" cho Android
   ```

4. **Build:**
   ```
   File > Build Settings
   - Add Open Scenes
   - Click "Build"
   - Chọn folder lưu APK
   ```

## 🎯 Controls

**Android touch** — cần tạo UI và gán references theo **BƯỚC 8**:
- Joystick — di chuyển; kéo AimLookArea — xoay/ngắm
- Fire — giữ để bắn; Reload — nạp đạn; Switch Weapon — đổi súng
- Heal Pack — dùng một pack; Shop/Pause — mở shop/tạm dừng

**PC**
- **WASD** - Di chuyển
- **Mouse** - Nhìn xung quanh
- **Left Mouse Button** - Bắn
- **R** - Reload
- **Space** - Nhảy
- **Left Shift** - Chạy nhanh
- **1, 2, 3** - Đổi vũ khí
- **Mouse Scroll** - Đổi vũ khí
- **ESC** - Pause menu
- **B** - Mở shop (có thể thêm button)

## 📱 Tối ưu cho Android

Tạo và nối joystick, vùng ngắm, Fire/Reload/Switch/Heal/Shop/Pause, reserve-ammo HUD và health-pack HUD theo **BƯỚC 8**. Logic component đã có; UI được dựng thủ công trong Unity để bạn tự quyết định giao diện.

## 🐛 Xử lý lỗi thường gặp

### Lỗi 1: Zombie không di chuyển
```
- Kiểm tra đã Bake NavMesh chưa
- Kiểm tra NavMeshAgent có enabled không
- Kiểm tra Player có tag "Player" không
```

### Lỗi 2: Không bắn được
```
- Kiểm tra Camera có tag "MainCamera" không
- Kiểm tra Gun script có reference đến muzzle flash không
- Kiểm tra Layer collision matrix
```

### Lỗi 3: UI không hiện
```
- Kiểm tra Canvas có Event System không
- Kiểm tra UIManager đã link các UI elements chưa
- Kiểm tra TextMeshPro đã import chưa
```

### Lỗi 4: Zombie không spawn
```
- Kiểm tra Zombie prefab có đầy đủ components
- Kiểm tra Player có tag `Player` và đã gán ít nhất một prefab hợp lệ vào Zombie Prefabs
- Kiểm tra NavMesh đã bake phủ quanh Player; bán kính spawn phải nằm trên vùng NavMesh
```

## 🎨 Customization

### Thêm vũ khí mới:
1. Import weapon model
2. Add Gun.cs component
3. Set các parameters (damage, fire rate, ammo)
4. Thêm vào WeaponSwitcher array
5. Thêm vào Shop nếu muốn bán

### Thêm loại zombie mới:
1. Kéo model zombie vào scene
2. Add NavMeshAgent, ZombieAI, ZombieHealth
3. Tùy chỉnh stats (health, damage, speed)
4. Tạo prefab
5. Thêm vào ZombieSpawner array

### Tùy chỉnh độ khó:
- Mở `GameManager.cs`
- Điều chỉnh `maxHealth`, `regenRate`
- Mở `ZombieSpawner.cs`
- Điều chỉnh `zombiesPerWave`, `spawnRate`
- Mở `ZombieAI.cs`
- Điều chỉnh `attackDamage`, `chaseSpeed`

## 📦 Dependencies

- Unity 6.0 (6000.6.5f1)
- TextMeshPro (built-in)
- Universal RP (URP)
- NavMesh Components (built-in)

## 👨‍💻 Credits

- Made with Unity 6
- Assets:
  - Zombie models: `Assets/Zombie/`
  - Weapons: `Low Poly Weapons VOL.1`
  - Environment: `Damaged Models pack1`
  - Player Controller: `Starter Assets`

## 📝 TODO / Features có thể thêm

- [ ] Boss zombie
- [ ] Power-ups (speed, damage boost)
- [ ] Multiple maps
- [ ] Leaderboard
- [ ] Sound effects đầy đủ
- [ ] Particle effects khi bắn
- [ ] Grenade/C4
- [ ] Mission system
- [ ] Multiplayer (nâng cao)

---

**Made for Unity Game Development Course**
Version: 1.0
Date: 2026
