# ✅ CHECKLIST SETUP GAME - Zombie Shooting

Checklist nhanh để setup game từ đầu. Đánh dấu ✓ khi hoàn thành.

---

## 🎯 PHASE 1: PROJECT SETUP

- [ ] Mở Unity Hub, chọn Unity 6.0 (6000.6.5f1)
- [ ] Mở project "ZombieShooting"

- [ ] **🔧 FIX INPUT SYSTEM (QUAN TRỌNG!):**
  - [ ] Edit > Project Settings > Player
  - [ ] Tab "Other Settings" > phần "Configuration"
  - [ ] "Active Input Handling" → chọn **"Input Manager (Old)"**
  - [ ] Unity hỏi restart → Click **Yes**
  - [ ] Đợi Unity restart xong
  - [ ] ✅ Lỗi Input đã fix!

- [ ] Kiểm tra scene mở đầu `Assets/Scenes/MainMenu.unity` và scene gameplay `Assets/Scenes/testscene.unity`
- [ ] Kiểm tra có các folder assets:
  - [ ] `Assets/Zombie/` (3 zombie prefabs)
  - [ ] `Assets/Low Poly Weapons VOL.1/` (weapon models)
  - [ ] `Assets/Damaged Models pack1/` (map props)
  - [ ] `Assets/Script/` (tất cả scripts đã tạo)

## 🎨 PHASE 2: MAIN MENU UI SETUP

`MainMenuController.cs`, `ShopUI.cs` và `WeaponShop.cs` chỉ xử lý logic. Tự tạo và thiết kế menu chính trong Editor; Armory dùng lại ShopMenu hiện có, không sinh UI bằng code.

### A. Canvas và panels
- [ ] Mở `Assets/Scenes/MainMenu.unity`
- [ ] Dùng Canvas và EventSystem đã có trong `MainMenu.unity`; chỉ tạo nếu scene thiếu
- [ ] Canvas Render Mode: **Screen Space - Overlay**
- [ ] Canvas Scaler:
  - [ ] UI Scale Mode: **Scale With Screen Size**
  - [ ] Reference Resolution: **1920 x 1080**
  - [ ] Match: **0.5**
- [ ] Dùng/chỉnh panel menu chính hiện có dưới Canvas; chỉ tạo nếu thiếu:
  - [ ] `MainMenuPanel` — Stretch full screen, active
- [ ] Tự thiết kế nền, logo/tiêu đề, màu, font, layout và kích thước cho màn hình ngang Android

### B. Tạo menu chính
- [ ] Dùng/chỉnh TextMeshPro UI `GameTitle`, `Subtitle`, `BalanceText` hiện có; chỉ tạo phần còn thiếu
- [ ] Dùng/chỉnh Button UI `PlayButton`, `ArmoryButton`, `QuitButton` hiện có; chỉ tạo phần còn thiếu
- [ ] Đặt các object này làm con của `MainMenuPanel`
- [ ] Đặt chữ/nội dung và style cho từng Text/Button theo thiết kế

### C. Tái sử dụng ShopMenu hiện có làm Armory
- [ ] Mở `testscene.unity`, dùng đúng object `ShopMenu`, component `ShopUI` và danh sách vũ khí trong `WeaponShop` hiện có; không tạo UI shop/buttons/catalog khác
- [ ] Kiểm tra catalog cũ: AK74 giá 500, Shotgun giá 1000, prefab súng và chỉ số đã đúng
- [ ] Bỏ `Weapon Holder` trên `WeaponShop`, kéo object này vào `Assets/Prefabs` để tạo prefab `WeaponShopCatalog`; gán lại `Weapon Holder` của instance trong `testscene` về WeaponHolder của Player
- [ ] Kéo `ShopMenu` vào `Assets/Prefabs` để tạo prefab UI `Armory`; giữ nguyên các nút vũ khí, Ammo, Health và Close
- [ ] Trong `testscene`, gán lại `ShopUI > Weapon Shop` tới instance `WeaponShopCatalog` trong cùng scene nếu prefab hóa làm mất reference
- [ ] Trong `MainMenu.unity`, đặt hai prefab instance `Armory` và `WeaponShopCatalog` dưới Canvas, cùng cấp với `MainMenuPanel`
- [ ] Đặt `Armory` inactive, gán `ShopUI > Weapon Shop` tới catalog instance trong MainMenu; để trống `Weapon Holder` của catalog này
- [ ] Đặt `ArmoryPanel` cũ trong MainMenu inactive hoặc xóa để không chồng lên prefab Armory mới
- [ ] Kiểm tra `ShopUI > Weapon Buttons`: đúng thứ tự AK74 rồi Shotgun; nút Close đã được gán
- [ ] Ammo/Health vẫn hiện và mua được cả trong Armory của MainMenu lẫn shop gameplay
- [ ] Tạo TMP `CreditsText` riêng trong Armory, gán vào `ShopUI > Credits Text` (không đổi tiêu đề SHOP)
- [ ] Đặt Ammo Pack Amount = 90, Ammo Pack Price = 50, Health Pack Price = 100

### D. Gắn MainMenuController
- [ ] Chọn object `MainMenu` trong Hierarchy (đã có `MainMenuController`); nếu tạo scene mới thì Create Empty tên `MainMenu` và Add Component
- [ ] Trong Inspector, kéo các references sau:

| Trường MainMenuController | Object/component cần kéo |
|---|---|
| Main Menu Panel | `MainMenuPanel` |
| Play Button | `PlayButton` |
| Armory Button | `ArmoryButton` |
| Quit Button | `QuitButton` |
| Balance Text (không bắt buộc) | TMP component trên `BalanceText` |
| Armory Shop | `ShopUI` component trên instance `Armory` |

- [ ] Trong `ShopUI` của Armory, gán `Weapon Shop` tới catalog instance của MainMenu
- [ ] Không cần khai báo Button OnClick thủ công; controller tự nối logic khi bắt đầu
- [ ] Lưu scene bằng **Ctrl+S**
- [ ] Chạy thử **ARMORY**, mua thử một món (credits/unlock lưu lại), **CLOSE** và **PLAY**

### E. Scenes In Build
- [ ] Mở **File > Build Profiles** (hoặc **Build Settings**)
- [ ] Thêm `Assets/Scenes/MainMenu.unity` và `Assets/Scenes/testscene.unity`
- [ ] Đặt `MainMenu.unity` ở đầu danh sách và bật cả hai scene
- [ ] Nút PLAY mở gameplay scene tên `testscene`

---

## 🗺️ PHASE 3: MAP SETUP

- [ ] Tạo Ground:
  - [ ] GameObject > 3D Object > Plane
  - [ ] Scale: (10, 1, 10)
  - [ ] Position: (0, 0, 0)
  - [ ] **Layer: "Ground"** (sẽ tạo layer này ở Phase 8, hoặc tạo ngay bây giờ - xem bên dưới)
  
- [ ] Thêm props từ Damaged Models:
  - [ ] Kéo vài prefab từ `Assets/Damaged Models pack1/Prefabs/`
  - [ ] Đặt rải rác trên map (cars, buildings, etc.)

- [ ] Bake NavMesh:
  - [ ] Window > AI > Navigation
  - [ ] Tab "Bake"
  - [ ] Agent Radius: 0.5
  - [ ] Agent Height: 2
  - [ ] Với NavMeshSurface đang dùng trong `testscene`, đặt **Use Geometry = Physics Colliders** nếu props có Collider nhưng mesh không được lấy làm vật cản
  - [ ] Đảm bảo Layer Mask của NavMeshSurface bao gồm Ground và các layer của vật cản; các vật cản tĩnh có Collider và nằm trong vùng bake
  - [ ] Click **"Bake"**
  - [ ] Kiểm tra vùng màu xanh không phủ xuyên qua tường/xe/vật cản; bake lại sau khi thay đổi props
- [ ] Với vật cản di chuyển trong lúc chơi, thêm `NavMeshObstacle`, bật **Carve**, và chỉnh kích thước/Center khớp Collider

---

## 🏃 PHASE 4: PLAYER SETUP

### A. Player GameObject
- [ ] Create Empty GameObject tên **"Player"**
- [ ] Position: (0, 1, 0)
- [ ] Tag: **"Player"**
- [ ] Layer: **"Player"**
- [ ] Add Component: **Character Controller**
  - [ ] Radius: 0.5
  - [ ] Height: 2
  - [ ] Center: (0, 1, 0)
- [ ] Add Component: **PlayerMovement.cs**
- [ ] Add Component: **PlayerHealth.cs**

### B. Camera
- [ ] Create Camera trong Player
- [ ] Rename: **"MainCamera"**
- [ ] Position: (0, 0.6, 0) [Mắt player]
- [ ] Tag: **"MainCamera"**
- [ ] Add Component: **MouseLook.cs**
- [ ] Kéo Player transform vào field **"Player Body"**

### C. Ground Check
- [ ] Create Empty GameObject trong Player
- [ ] Rename: **"GroundCheck"**
- [ ] Position: (0, 0, 0) [Dưới chân]
- [ ] Chọn Player > PlayerMovement
- [ ] Kéo GroundCheck vào field **"Ground Check"**
- [ ] Ground Mask: chọn layer **"Ground"** (tạo ở Phase 8 - Final Settings)

### D. Weapon Holder
- [ ] Create Empty GameObject trong MainCamera
- [ ] Rename: **"WeaponHolder"**
- [ ] Position: (0.3, -0.3, 0.5)
- [ ] Add Component: **WeaponSwitcher.cs**

### E. Add Weapon
- [ ] Kéo `Assets/Low Poly Weapons VOL.1/Prefabs/M1911.prefab` vào WeaponHolder
- [ ] Chọn M1911, adjust position để nhìn đẹp:
  - [ ] Rotation: (0, -90, 0) hoặc tùy chỉnh
  - [ ] Scale: (1, 1, 1)
- [ ] Add Component: **Gun.cs**
- [ ] Add Component: **Audio Source**
- [ ] Setup Gun.cs:
  - [ ] Gun Name: "Pistol"
  - [ ] Damage: 25
  - [ ] Fire Rate: 0.2
  - [ ] Max Ammo: 30
  - [ ] Reload Time: 1.5
  - [ ] Range: 100
- [ ] Chọn WeaponHolder > WeaponSwitcher
- [ ] Set Weapons size = 1
- [ ] Kéo M1911 vào Element 0

---

## 🧟 PHASE 5: ZOMBIE SETUP

### A. Setup Zombie Prefab
- [ ] Gameplay đang dùng `Assets/Zombie/Prefabs/zombiechuan.prefab`; chỉnh prefab này nếu cần sửa AI hoặc va chạm
- [ ] Add Component: **NavMeshAgent**
  - [ ] Speed: 2
  - [ ] Angular Speed: 120
  - [ ] Acceleration: 8
  - [ ] Stopping Distance: 2
- [ ] Trên Animator, tắt **Apply Root Motion** để chỉ NavMeshAgent điều khiển dịch chuyển
- [ ] Add Component: **ZombieAI.cs**
  - [ ] Detection Range: 15
  - [ ] Attack Range: 2
  - [ ] Walk Speed: 2
  - [ ] Chase Speed: 3.5
  - [ ] Attack Damage: 10
  - [ ] Attack Rate: 1
- [ ] Add Component: **ZombieHealth.cs**
  - [ ] Max Health: 100
  - [ ] Money Reward: 50
- [ ] Tag: **"Enemy"**
- [ ] Layer: **"Enemy"**
- [ ] Add Capsule Collider nếu chưa có:
  - [ ] Radius: 0.5
  - [ ] Height: 2

### B. Update Prefab
- [ ] Kéo zombie từ scene vào `Assets/Zombie/Prefabs/`
- [ ] Overwrite prefab
- [ ] Delete zombie trong scene

### C. Zombie Spawner
- [ ] Create Empty GameObject: **"ZombieSpawner"**
- [ ] Position: (0, 0, 0)
- [ ] Add Component: **ZombieSpawner.cs**

- [ ] Chọn ZombieSpawner:
  - [ ] Zombie Prefabs: Size = 1
    - [ ] Kéo Zombie1 prefab vào
  - [ ] Max Zombies đồng thời: 20
  - [ ] Spawn Rate: 0.5 giây
  - [ ] Zombies Per Wave: 20 (mỗi wave đều 20)
  - [ ] Spawn quanh Player: cách 18–30 units; NavMesh Sample Distance: 5; Attempts: 30
  - [ ] Enable Waves: ✓ Check
  - [ ] Time Between Waves: 8 giây
  - [ ] Walk Speed Increase Per Wave: 0.15
  - [ ] Chase Speed Increase Per Wave: 0.25
  - [ ] Hoàn thành đủ 10 wave; mỗi wave spawn 20 zombie và chiến thắng chỉ hiện sau khi hạ hết wave cuối
- [ ] Scene `testscene` có 4 GroundTile đặt ghép 2×2 (kích thước tổng khoảng 200×200 Unity units); sau khi thay đổi map, bake lại NavMeshSurface và xác nhận NavMesh không tràn khỏi map
- [ ] PlayerMovement giới hạn Player trong X/Z: min (-48, -48), max (148, 148)

---

## 🎮 PHASE 6: MANAGERS SETUP

### A. Game Manager
- [ ] Create Empty GameObject: **"GameManager"**
- [ ] Add Component: **GameManager.cs**
- [ ] Player Money: 500 (starting money)

### B. Audio Manager
- [ ] Create Empty GameObject: **"AudioManager"**
- [ ] Add Component: **Audio Source** (2 lần)
- [ ] Add Component: **AudioManager.cs**
- [ ] Kéo Audio Source đầu vào **"Music Source"**
- [ ] Kéo Audio Source thứ 2 vào **"SFX Source"**

---

## 🖥️ PHASE 7: UI SETUP (GAMEPLAY HUD)

### A. Create Canvas
- [ ] UI > Canvas
- [ ] Canvas Scaler:
  - [ ] UI Scale Mode: **Scale With Screen Size**
  - [ ] Reference Resolution: **1920 x 1080**
- [ ] Kiểm tra có **EventSystem** trong scene

### B. HUD - Health Bar
- [ ] Trong Canvas, tạo UI > Image: **"HealthBarBG"**
  - [ ] Anchor: Top Left
  - [ ] Position: (150, -30, 0)
  - [ ] Width: 200, Height: 20
  - [ ] Color: Dark Gray/Black
  
- [ ] Tạo UI > Image con: **"HealthBarFill"**
  - [ ] Anchor: Stretch (full)
  - [ ] Color: Red/Green
  - [ ] Image Type: **Filled**
  - [ ] Fill Method: **Horizontal**
  - [ ] Fill Amount: 1

### C. HUD - Text Elements
Import TextMeshPro nếu Unity hỏi (Import TMP Essentials)

- [ ] UI > Text - TextMeshPro: **"MoneyText"**
  - [ ] Anchor: Top Right
  - [ ] Position: (-150, -30, 0)
  - [ ] Text: "$0"
  - [ ] Font Size: 36
  - [ ] Alignment: Right
  
- [ ] UI > Text - TextMeshPro: **"AmmoText"**
  - [ ] Anchor: Bottom Right
  - [ ] Position: (-150, 80, 0)
  - [ ] Text: "30 / 30"
  - [ ] Font Size: 30
  
- [ ] UI > Text - TextMeshPro: **"KillsText"**
  - [ ] Anchor: Bottom Left
  - [ ] Position: (150, 30, 0)
  - [ ] Text: "Kills: 0"
  - [ ] Font Size: 24
  
- [ ] UI > Text - TextMeshPro: **"WaveText"**
  - [ ] Anchor: Top Center
  - [ ] Position: (0, -30, 0)
  - [ ] Text: "Wave: 1"
  - [ ] Font Size: 32
  - [ ] Alignment: Center

### D. HUD - Crosshair
- [ ] UI > Image: **"Crosshair"**
  - [ ] Anchor: Center
  - [ ] Position: (0, 0, 0)
  - [ ] Width: 20, Height: 20
  - [ ] Color: White, Alpha: 0.7
  - [ ] Sprite: Chọn circle hoặc crosshair (UI > Sprite > Knob)

### E. Pause Menu
- [ ] UI > Panel: **"PauseMenu"**
  - [ ] Color: Black, Alpha: 0.8
  
- [ ] Trong PauseMenu:
  - [ ] UI > Text: "PAUSED" (center, size 48)
  - [ ] UI > Button: **"ResumeButton"** - Text: "Resume"
  - [ ] UI > Button: **"RestartButton"** - Text: "Restart"
  - [ ] UI > Button: **"QuitButton"** - Text: "Quit"
  
- [ ] **Ẩn PauseMenu:** Uncheck ở Inspector

### F. Game Over Menu
- [ ] UI > Panel: **"GameOverMenu"**
  - [ ] Color: Dark Red, Alpha: 0.9
  
- [ ] Trong GameOverMenu:
  - [ ] UI > Text: **"GameOverTitle"** - "GAME OVER" (size 56)
  - [ ] UI > Text: **"FinalKillsText"** - "Total Kills: 0"
  - [ ] UI > Text: **"FinalWaveText"** - "Wave Reached: 1"
  - [ ] UI > Button: **"RestartButton"** - Text: "Restart"
  - [ ] UI > Button: **"QuitButton"** - Text: "Main Menu"
  
- [ ] **Ẩn GameOverMenu:** Uncheck ở Inspector

### G. Shop Menu
- [ ] Dùng object **ShopMenu** hiện có trong `testscene`; không tạo lại panel/nút
- [ ] Kiểm tra `ShopUI` đã tham chiếu đủ weapon buttons, Ammo, Health, Close
- [ ] Dùng lại `WeaponShop` catalog có sẵn; không tạo danh sách súng khác
- [ ] Gán đúng **AK74 - $500** và **Shotgun - $1000** qua dữ liệu catalog cũ
- [ ] Trong MainMenu, tạo instance từ prefab `Armory` theo Phase 2; Ammo/Health vẫn hiện và mua được
- [ ] **Ẩn ShopMenu gameplay:** Uncheck ở Inspector

### H. Setup UI Manager
- [ ] Chọn Canvas
- [ ] Add Component: **UIManager.cs**
- [ ] Kéo tất cả UI elements vào fields:
  - [ ] Money Text
  - [ ] Ammo Text
  - [ ] Kills Text
  - [ ] Wave Text
    - [ ] Hiện số wave hiện tại trên tổng 10 và thời gian sinh tồn `MM:SS`
  - [ ] Health Bar (fill)
  - [ ] Crosshair
  - [ ] Pause Menu
  - [ ] Game Over Menu
  - [ ] Shop Menu
  - [ ] Final Kills Text
  - [ ] Final Wave Text
    - [ ] Final Kills Text hiển thị số kill và Survival Time
    - [ ] Kết thúc đủ 10 wave hiện `VICTORY!`; chết giữa trận hiện `GAME OVER`
  - [ ] Mobile Controls Root
  - [ ] Reserve Ammo Text
  - [ ] Health Packs Text

---

## 📱 PHASE 8: MOBILE CONTROLS, HUD & SHOP INVENTORY

### A. Mobile controls/HUD — dựng UI thủ công trong Unity
- [ ] Dưới Canvas gameplay, tạo `MobileControlsRoot` (active khi gameplay chạy)
- [ ] Tạo Image `MoveJoystick` neo góc dưới-trái, với Image con `Handle` neo giữa
- [ ] Tạo Image trong suốt `AimLookArea` ở vùng trống bên phải/trung tâm; bật Raycast Target và không che nút
- [ ] Tạo Button `FireButton`, `ReloadButton`, `SwitchWeaponButton`, `HealPackButton`, `OpenShopButton`, `PauseButton`; đặt anchors/kích thước thuận thao tác ngón cái
- [ ] Tạo TMP `ReserveAmmoText` và `HealthPacksText`; giữ AmmoText hiện có cho số đạn trong băng
- [ ] Add `VirtualJoystick` vào `MoveJoystick`, gán Background/Handle; kéo component vào `PlayerMovement > Mobile Joystick`
- [ ] Add `MobileCombatControls` vào `MobileControlsRoot`, gán WeaponSwitcher, PlayerHealth, Reload/Switch/Heal/Shop/Pause Buttons
- [ ] Tạo `JumpButton` (Button), đặt gần joystick/cụm điều khiển, thêm nhãn `JUMP`; gán Button component vào `MobileCombatControls > Jump Button` và gán `PlayerMovement` của Player vào field cùng tên
- [ ] Không thêm Button OnClick thủ công (MobileCombatControls tự đăng ký); đảm bảo Button graphic nhận raycast qua Canvas/EventSystem
- [ ] Nhấn Jump chỉ khi Player đang chạm đất; kiểm tra cả nút cảm ứng và phím Space trong Editor
- [ ] Add `MobileFireButton` vào `FireButton`, gán `MobileCombatControls`
- [ ] Add `TouchLookArea` vào `AimLookArea`, gán `MouseLook` của camera
- [ ] Trong UIManager, gán Mobile Controls Root, Reserve Ammo Text, Health Packs Text
- [ ] Kiểm tra Canvas có GraphicRaycaster và scene có EventSystem/InputSystemUIInputModule
- [ ] Tạo Settings Panel riêng trong menu và gameplay, mỗi panel có hai Slider (Music/SFX) và nút Close; thêm AudioSettingsUI lên object active và gán các references
- [ ] Menu: gán AudioSettingsUI vào MainMenuController; nút Setting có sẵn sẽ mở panel
- [ ] Pause: duplicate/copy kiểu nút Setting vào PauseMenu, gán nút và AudioSettingsUI vào hai field tương ứng trong UIManager
- [ ] Slider Music/SFX đặt Min 0, Max 1, Whole Numbers off; kiểm tra hai mức âm lượng độc lập và lưu qua scene
- [ ] Gán AudioSource và clip nhạc nền cho AudioManager nếu muốn nghe Music slider hoạt động
- [ ] Để nút Switch đổi được súng, `WeaponSwitcher > Weapons` phải có ít nhất 2 Gun hợp lệ; hiện tại scene mới gán M1911, nên thêm AK74/Shotgun vào Element tiếp theo hoặc mua các khẩu đó trong Armory
- [ ] Nếu thêm model thủ công để test, căn local position/rotation/scale riêng cho từng khẩu; lưu ý cách này cho phép dùng súng mà không cần mua
- [ ] Trên APK Android mới, xác nhận chạm ngoài Fire không làm đạn giảm; chỉ giữ Fire mới bắn

### B. Kiểm tra shop và inventory dùng chung
- [ ] Dùng `WeaponShopCatalog` và `Armory/ShopMenu` hiện có; không tạo shop/catalog thứ hai
- [ ] Mỗi scene gán ShopUI tới catalog instance trong scene đó; chỉ gameplay gán Weapon Holder
- [ ] Giữ AK74 $500 và Shotgun $1000
- [ ] ShopUI: Ammo Pack Amount = 90, Ammo Pack Price = 50; Health Pack Price = 100
- [ ] ShopUI gán Ammo, Health, Close Buttons cùng Weapon Buttons hiện có
- [ ] Tạo TMP `CreditsText` riêng và gán vào field ShopUI; không ghi đè title SHOP
- [ ] Buttons cập nhật số reserve ammo và health packs hiện có
- [ ] Mua ammo/health ở MainMenu và gameplay đều cập nhật PlayerPrefs, HUD và label inventory
- [ ] Người chơi mới có 90 reserve ammo và 1 health pack; Reload tiêu thụ reserve, HEAL tiêu thụ một pack và hồi 50 HP
- [ ] M1911 mặc định có đạn dự trữ vô hạn; các súng AK74/Shotgun vẫn dùng đạn mua trong Armory
- [ ] HEAL không tiêu thụ item khi Player đủ máu hoặc hết inventory

---

## 🔧 PHASE 9: FINAL SETTINGS

### A. Layers & Tags

**Tạo Tags:**
- [ ] Edit > Project Settings > Tags and Layers
- [ ] Tab "Tags"
- [ ] Click + để add tag mới:
  - [ ] Add Tag: **Player**
  - [ ] Add Tag: **Enemy**

**Tạo Layers:**
- [ ] Vẫn trong Tags and Layers window
- [ ] Tab "Layers"
- [ ] Tìm Layer trống (thường User Layer 6, 7, 8)
- [ ] Nhập tên:
  - [ ] Layer 6: **Player**
  - [ ] Layer 7: **Enemy**
  - [ ] Layer 8: **Ground**

**Assign Layers cho GameObjects:**
- [ ] Chọn Plane (ground) > Inspector > Layer dropdown > chọn **Ground**
- [ ] Chọn Player GameObject > Layer > **Player**
- [ ] Chọn Zombie prefab > Layer > **Enemy**

### B. Physics Collision Matrix
- [ ] Edit > Project Settings > Physics
- [ ] Layer Collision Matrix:
  - [ ] Uncheck: Player vs Player
  - [ ] Uncheck: Enemy vs Enemy

### C. Input Settings
- [ ] Edit > Project Settings > Input Manager
- [ ] Kiểm tra có các inputs:
  - [ ] Horizontal (A/D, Arrow)
  - [ ] Vertical (W/S, Arrow)
  - [ ] Fire1 (Mouse Button 0)
  - [ ] Jump (Space)
  - [ ] Mouse X
  - [ ] Mouse Y

---

## ✅ PHASE 10: TEST GAME

### A. Play Mode Test
- [ ] Mở `MainMenu.unity`, chạy Play Mode và dùng Button UI trên Canvas
- [ ] Chọn PLAY để load scene gameplay
- [ ] Trong gameplay scene, test điều khiển và HUD
- [ ] Test di chuyển (WASD)
- [ ] Test nhìn xoay (Mouse)
- [ ] Test nhảy (Space)
- [ ] Test bắn (Left Click)
- [ ] Đợi zombie spawn
- [ ] Test bắn zombie
- [ ] Kiểm tra UI update (money, kills, health)
- [ ] Test pause (ESC)
- [ ] Test chết (để zombie đánh nhiều lần)

### B. Checklist khi test:
- [ ] Player di chuyển mượt
- [ ] Camera xoay theo chuột
- [ ] Súng bắn được
- [ ] Zombie spawn và đuổi theo
- [ ] Zombie tấn công làm giảm máu
- [ ] Bắn zombie tăng tiền và kills
- [ ] Health bar giảm khi bị đánh
- [ ] Ammo giảm khi bắn
- [ ] Pause menu hoạt động
- [ ] Game over khi hết máu

### C. Android UI Test
- [ ] Chạy thử trên thiết bị hoặc Android Emulator
- [ ] Kiểm tra menu/armory xoay ngang và UI không bị cắt ở các tỉ lệ màn hình
- [ ] Tạo/nối đầy đủ các touch controls theo Phase 8
- [ ] Test joystick, vùng ngắm cảm ứng, giữ Fire, Reload, Switch, Heal, Shop và Pause trên thiết bị
- [ ] Kiểm tra HUD reserve ammo/health packs và inventory sau khi mua, reload, dùng heal pack

---

## 🐛 TROUBLESHOOTING

### Nếu có lỗi:

**Zombie không di chuyển:**
- [ ] Kiểm tra NavMesh đã bake
- [ ] Kiểm tra Player có tag "Player"
- [ ] Kiểm tra NavMeshAgent enabled

**Không bắn được:**
- [ ] Kiểm tra Camera tag "MainCamera"
- [ ] Kiểm tra Gun có AudioSource
- [ ] Kiểm tra currentAmmo > 0

**UI không hiện:**
- [ ] Kiểm tra Canvas có EventSystem
- [ ] Kiểm tra UIManager đã link đúng
- [ ] Nếu menu báo `MainMenuController is missing its Inspector reference`, gán đủ references theo bảng ở Phase 2
- [ ] Kiểm tra TextMeshPro imported

**Console có lỗi NullReferenceException:**
- [ ] Kiểm tra tất cả public fields đã assign
- [ ] Kiểm tra FindObjectOfType() có tìm thấy không

---

## 📱 PHASE 11: BUILD FOR ANDROID (OPTIONAL)

- [ ] File > Build Settings
- [ ] Platform: Android
- [ ] Click "Switch Platform"
- [ ] Player Settings:
  - [ ] Company Name: Tên của bạn
  - [ ] Product Name: "Zombie Shooting"
  - [ ] Package Name: com.yourname.zombieshooting
  - [ ] Minimum API Level: Android 7.0 (API 24)
  - [ ] Scripting Backend: IL2CPP
  - [ ] Target Architectures: ARM64 ✓
- [ ] Add Open Scenes
- [ ] Click "Build"
- [ ] Chọn folder và tên file APK
- [ ] Đợi build xong
- [ ] Test APK trên điện thoại

---

## 🎉 DONE!

Game hoàn thành! Bạn có thể:
- [ ] Thêm nhiều loại zombie
- [ ] Thêm nhiều vũ khí
- [ ] Thêm power-ups
- [ ] Thêm nhiều map
- [ ] Thêm boss zombie
- [ ] Thêm sound effects
- [ ] Polish UI đẹp hơn

**Estimated Setup Time:** 1-2 hours

**Chúc bạn làm game thành công! 🎮**
