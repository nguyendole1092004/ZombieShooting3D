# 🚀 QUICK START - Bắt đầu ngay!

**Dành cho người vừa clone/download project này.**

---

## ⚡ Mở và chạy game

### **Bước 1: Mở đúng project**

```
1. Mở Unity Hub
2. Chọn Add > Add project from disk
3. Chọn thư mục ZombieShooting
4. Mở bằng Unity 6.0 (6000.6.5f1)
5. Chờ Unity import assets và compile scripts xong
```

Project đã cấu hình Input Handling là **Both**. Không cần đổi setting này nếu game không báo lỗi input.

### **Bước 2: Kiểm tra scene**

```
1. Trong Project window, mở Assets/Scenes/MainMenu.unity
2. Bấm Play để chạy menu
3. Bấm PLAY để vào trận hoặc ARMORY để mở shop
```

Gameplay scene `Assets/Scenes/testscene.unity` đã có sẵn map, player, zombie spawner, HUD và shop trong game. Không cần tạo lại các object này nếu đang dùng scene hiện tại.

Map gameplay gồm bốn GroundTile ghép vuông 2×2 (khoảng 200×200 Unity units). Zombie spawn ngẫu nhiên trên NavMesh thành vòng quanh Player, cách khoảng 18–30 Unity units; các SpawnPoint cũ không còn được dùng. Player bị giới hạn trong vùng X/Z từ -48 đến 148 để không chạy khỏi map. Sau khi mở scene hoặc chỉnh props, chọn `Plane` có `NavMeshSurface`, bake lại NavMesh và xác nhận vùng bake chỉ phủ các nền.

Trận đấu có tối đa **10 wave**, mỗi wave spawn đúng **20 zombie**, tối đa 20 con sống đồng thời và nhịp spawn 0.5 giây/con. Giữa các wave nghỉ 8 giây. Số lượng giữ nguyên ở mọi wave; tốc độ đi tăng 0.15 và tốc độ đuổi tăng 0.25 mỗi wave. Hạ hết một wave thì wave kế tiếp mới bắt đầu; hạ hết wave 10 sẽ hiện `VICTORY!`. HUD hiển thị wave và thời gian `MM:SS`; khi chết hoặc thắng, màn hình cuối hiện tổng kill, wave và thời gian sinh tồn.

### **Bước 3: Chơi hoặc build Android**

Trong Unity 6, mở **File > Build Profiles**, chọn **Android**, rồi chọn **Build**. Nếu Unity yêu cầu Android Build Support, cài module cho đúng phiên bản Unity trong Unity Hub trước.

> Gameplay đã có logic touch controls; tạo các UI object và nối references trong Inspector theo mục **F. Tạo touch controls cho Android** bên dưới trước khi build Android.

---

## 🛠️ Tự tạo Main Menu và Armory trong Unity Editor

`MainMenuController.cs`, `ShopUI.cs` và `WeaponShop.cs` chỉ chứa logic; **không tự tạo Canvas, panel hay nút**. Hãy tự dựng menu chính trong Editor, còn phần Armory dùng ShopMenu prefab hiện có để tùy chỉnh style, hình nền, font, spacing và animation. Scene `MainMenu.unity` hiện có object `MainMenu` với script; tất cả các trường cần kéo vào Inspector.

### A. Tạo responsive Canvas

1. Mở `Assets/Scenes/MainMenu.unity`. Scene hiện đã có Canvas, MainMenuPanel, các nút chính và một ArmoryPanel cũ; hãy tận dụng/chỉnh thiết kế các object có sẵn, không tạo Canvas hay nút trùng.
2. Trong **Hierarchy**, chỉ chọn **+ > UI > Canvas** nếu scene chưa có Canvas. Unity tự thêm `EventSystem`; chỉ tạo qua **+ > UI > Event System** nếu chưa có.
3. Chọn `Canvas`, đặt **Render Mode** = `Screen Space - Overlay`.
4. Trong **Canvas Scaler**, chọn **UI Scale Mode** = `Scale With Screen Size`, **Reference Resolution** = `1920 x 1080`, **Match** = `0.5`.
5. Với Android landscape, dùng anchors để giữ các thành phần đúng vị trí ở nhiều tỉ lệ màn hình; chừa khoảng cách khỏi mép để tránh camera cutout/thanh điều hướng.

### B. Tạo giao diện menu chính và tái sử dụng ShopMenu
Trong Canvas, dùng `MainMenuPanel`, `GameTitle`, `Subtitle`, `BalanceText`, `PlayButton`, `ArmoryButton`, `QuitButton` đang có nếu còn trong scene; tạo thủ công trong Editor chỉ những object còn thiếu. Chọn import TMP Essentials nếu Unity hỏi.

```text
Canvas
├── MainMenuPanel (Panel, Stretch full screen)
│   ├── Background (Image: artwork của bạn)
│   ├── GameTitle (TextMeshPro)
│   ├── Subtitle (TextMeshPro)
│   ├── BalanceText (TextMeshPro: CREDITS $500)
│   ├── PlayButton (Button: PLAY)
│   ├── ArmoryButton (Button: ARMORY)
│   └── QuitButton (Button: QUIT)
├── Armory (ShopMenu cũ, inactive lúc bắt đầu)
└── WeaponShop (prefab catalog dùng chung, không cần UI)
```

`MainMenuPanel` bắt đầu active; `Armory` bắt đầu inactive. Đặt `ArmoryPanel` cũ trong MainMenu inactive hoặc xóa nó để tránh chồng lên shop mới. Tự thiết kế riêng giao diện Main Menu. **Không tạo lại các nút shop**: dùng ShopMenu hiện có ở `testscene` làm Armory.

#### Đưa ShopMenu và dữ liệu shop cũ vào MainMenu
1. Mở `Assets/Scenes/testscene.unity`. Tìm object `WeaponShop` và kiểm tra có 2 item cũ: AK74 ($500), Shotgun ($1000), đúng prefab và thông số súng.
2. Trên `WeaponShop`, bỏ reference `Weapon Holder` trước khi tạo prefab để catalog dùng được ở cả hai scene. Kéo object `WeaponShop` từ Hierarchy vào `Assets/Prefabs` (tạo folder nếu chưa có), đặt tên `WeaponShopCatalog`; Unity sẽ biến object trong scene thành prefab instance. Gán lại `Weapon Holder` của instance này về WeaponHolder của Player.
3. Kéo object `ShopMenu` từ Hierarchy vào `Assets/Prefabs`, đặt tên `Armory`. Đây là prefab hóa UI shop đang dùng; không tạo ShopUI/buttons mới. Trong `testscene`, đảm bảo field **Shop UI > Weapon Shop** đang trỏ tới instance `WeaponShopCatalog`.
4. Mở `MainMenu.unity`, kéo prefab `Armory` và `WeaponShopCatalog` vào bên trong Canvas, cùng cấp với `MainMenuPanel`. Đổi tên instance UI thành `Armory`, đặt inactive. Mở `Armory` và gán field **Shop UI > Weapon Shop** tới instance `WeaponShopCatalog` trong MainMenu.
5. Trong instance `WeaponShopCatalog` ở MainMenu, để trống **Weapon Holder**. Khi không có Player, shop chỉ lưu mở khóa/credits; khi vào gameplay, WeaponShop sẽ nạp súng đã mua vào loadout. ShopMenu prefab có thể mất reference `Weapon Shop` khi tạo prefab; cần gán lại trong cả hai scene như trên.
6. Tạo TextMeshPro UI `CreditsText` riêng trong Armory để hiện credits mà không ghi đè tiêu đề SHOP. `BalanceText` trong `MainMenuPanel` có thể dùng để hiện số dư ở menu chính.

Tự thiết kế artwork, màu, font, kích thước nút, anchors và layout cho menu chính; prefab Armory giữ nguyên giao diện shop hiện có. Credits, đạn dự trữ và health pack dùng chung dữ liệu lưu `PlayerPrefs` với gameplay. Giữ nút Ammo/Health ở Armory cả menu lẫn gameplay: Ammo mua **90 viên dự trữ / $50**, Health Pack mua **1 pack hồi 50 HP / $100**. Tạo TMP text riêng `CreditsText` trong Armory và gán vào `ShopUI > Credits Text`; các nút hiển thị số lượng inventory hiện có. Nút Close trong menu quay lại menu chính.

### C. Gắn logic và kéo references vào Inspector
1. Scene có sẵn object `MainMenu` với `MainMenuController`. Nếu tạo scene mới, tạo **Create Empty** tên `MainMenu` rồi **Add Component > MainMenuController**.
2. Chọn `MainMenu`, kéo object/component tương ứng vào từng ô của script:

| Trường trong Inspector | Kéo object/component này vào |
|---|---|
| Main Menu Panel | `MainMenuPanel` |
| Play Button | `PlayButton` |
| Armory Button | `ArmoryButton` |
| Quit Button | `QuitButton` |
| Balance Text | TMP component trên `BalanceText` (có thể để trống nếu không muốn hiện balance ở menu chính) |
| Armory Shop | component `ShopUI` trên instance `Armory` |

Trong `ShopUI` của Armory, kiểm tra `Weapon Buttons` giữ đúng thứ tự AK74 rồi Shotgun; `Weapon Shop` phải trỏ tới catalog instance cùng scene. Kéo TMP component `CreditsText` vào trường ShopUI > Credits Text. Không cần khai báo Button OnClick thủ công: script nối các sự kiện khi bắt đầu. Shop tiếp tục dùng giá/dữ liệu cũ, trừ credits và lưu unlock giữa các lần chơi. Credits mặc định là **$500**; diệt zombie kiếm thêm tiền.

### D. Build Settings và kiểm tra

1. Mở **File > Build Profiles** (hoặc **File > Build Settings**).
2. Thêm `Assets/Scenes/MainMenu.unity` và `Assets/Scenes/testscene.unity` vào **Scenes In Build**.
3. Đặt `MainMenu` ở đầu danh sách và bật cả hai scene.
4. Lưu scene bằng **Ctrl+S**, chạy `MainMenu.unity`, thử ARMORY rồi PLAY.

Nút PLAY mở scene `testscene`. Nút Quit ở Pause/Game Over quay về `MainMenu` và không thoát ứng dụng; Quit trên menu chính mới thoát game. Nếu Console báo `MainMenuController is missing its Inspector reference`, gán field còn thiếu theo bảng trên. Nếu đổi tên gameplay scene, đổi hằng `GameplayScene` trong `Assets/Script/UI/MainMenuController.cs`; nếu đổi tên Main Menu scene, cập nhật `MainMenuScene` trong `GameManager.cs`.

### E. Gameplay scene hiện tại

`Assets/Scenes/testscene.unity` đã có Player, HUD, zombie spawner và shop. Để tự dựng/chỉnh các module gameplay hoặc làm map/UI đẹp hơn, tham khảo [SETUP_CHECKLIST.md](SETUP_CHECKLIST.md) và kéo các references vào Inspector.

### F. Thêm súng để nút Switch đổi vũ khí

Trong scene hiện tại, `WeaponHolder > WeaponSwitcher > Weapons` mới có **M1911 ở Element 0**. Nút Switch chỉ chuyển được khi mảng này có ít nhất hai `Gun` hợp lệ. Weapon holder là con của camera, nên các model đặt bên trong sẽ hiện ở góc pandang FPS; không cần tạo nhân vật thứ ba để thấy súng. Prefab trong thư mục vũ khí là model súng; chỉnh vị trí, xoay và tỷ lệ của từng instance để căn vào góc nhìn.

#### Cách A — thêm súng miễn phí để thử đổi vũ khí

1. Mở `Assets/Scenes/testscene.unity`, mở rộng Player → Camera → WeaponHolder.
2. Kéo `Assets/Low Poly Weapons VOL.1/Prefabs/AK74.prefab` vào `WeaponHolder`. Đổi tên instance `AK74`.
3. Thêm component `Gun` nếu prefab chưa có; không thêm bản thứ hai nếu đã có. Đặt `Gun Name = AK74`, cấu hình chỉ số thử nghiệm; thêm Audio Source/hiệu ứng nếu muốn.
4. Reset local position/rotation của model, rồi chỉnh Transform trong Play Mode/Scene View để súng nằm đúng góc dưới màn hình. Mỗi instance có thể cần vị trí và tỷ lệ riêng.
5. Kéo component `Gun` của AK74 vào `WeaponSwitcher > Weapons > Size = 2 > Element 1`.
6. Để có khẩu thứ ba, làm tương tự với `Assets/Low Poly Weapons VOL.1/Prefabs/Bennelli_M4.prefab`, tên `Shotgun`, rồi gán vào Element 2. Có thể dùng prefab/model khác; tên hiển thị nên khớp catalog.
7. Giữ đúng thứ tự mảng: **M1911, AK74, Shotgun**. Khi chạy, `WeaponSwitcher` chỉ bật model ở slot đang chọn; nút Switch chuyển tới slot hợp lệ kế tiếp. Trên PC có thể thử phím 1/2/3 hoặc con lăn chuột.

> Cách này cho phép dùng mọi khẩu đã gán mà không cần mua. Nếu cần giữ shop làm điều kiện mở khóa, dùng Cách B thay vì thêm trước cùng một vũ khí: nếu vừa đặt sẵn AK74/Shotgun vào mảng vừa mua trong shop, catalog có thể tạo thêm một bản runtime trùng.

#### Cách B — giữ nguyên điều kiện mua trong Armory

1. Giữ `WeaponSwitcher > Weapons` chỉ có M1911 ban đầu.
2. Trong `WeaponShopCatalog` của gameplay, kiểm tra AK74 và Shotgun đang trỏ đúng prefab, giá và chỉ số; `Weapon Holder` phải trỏ tới WeaponHolder của Player. Shop ở MainMenu để trống holder như cấu hình hiện tại.
3. Mua AK74/Shotgun ở Armory. `WeaponShop` sẽ tạo khẩu đã mở khóa làm con của WeaponHolder và thêm `Gun` đó vào cuối mảng Weapons. Nút Switch hoạt động khi có ít nhất một khẩu khác M1911 đã mua.
4. Kiểm tra vị trí/rotation của model được tạo trong Play Mode. Nếu catalog prefab không đặt vừa tầm nhìn FPS, chỉnh transform của prefab/model trong Project hoặc dùng Cách A để kiểm tra rồi lưu transform phù hợp vào prefab.

#### Nếu nhấn Fire vẫn tự bắn

- Trong APK Android, bắn chuột `Fire1` đã bị vô hiệu hóa; bắn chỉ nhận từ `FireButton` trên UI. Cài APK build mới sau thay đổi code, vì APK đang có trên điện thoại không tự cập nhật.
- Trong Unity Editor/PC, giữ chuột trái vẫn là điều khiển bắn; click chỗ trống để test UI không được hiểu là test Android. Trên điện thoại, chỉ giữ nút Fire mới được bắn.
- Kiểm tra `FireButton` có `Button` và `MobileFireButton`, field `Controls` được gán tới `MobileControlsRoot > MobileCombatControls`; đảm bảo không có script/component khác đang gọi bắn ngoài các thành phần này.

### G. Tạo touch controls cho Android

Trong `Assets/Scenes/testscene.unity`, dùng Canvas gameplay đã có. Không cần tạo UI bằng code; tự tạo các Image/Button dưới Canvas và đặt anchors/kích thước phù hợp màn hình ngang:

```text
Canvas
├── HUD
│   ├── AmmoText             (đạn trong băng / sức chứa)
│   ├── ReserveAmmoText      (đạn dự trữ còn lại)
│   ├── HealthPacksText      (HEAL x số pack)
│   └── HealthBarFill        (đã có)
└── MobileControlsRoot       (active khi chơi)
    ├── AimLookArea          (Image trong vùng trống bên phải; raycast target bật)
    ├── MoveJoystick         (Image, neo góc dưới-trái)
    │   └── Handle           (Image con, neo giữa)
    ├── FireButton            (Button, góc dưới-phải)
    ├── JumpButton            (Button, gần cụm nút điều khiển bên phải)
    ├── ReloadButton          (Button)
    ├── SwitchWeaponButton    (Button)
    ├── HealPackButton        (Button: HEAL xN)
    ├── OpenShopButton        (Button)
    └── PauseButton           (Button)
```

1. Thêm `VirtualJoystick` vào `MoveJoystick`; gán `Background` = RectTransform của joystick, `Handle` = RectTransform của con `Handle`. Kéo component `VirtualJoystick` vào `Player > PlayerMovement > Mobile Joystick`.
2. Thêm `MobileCombatControls` lên `MobileControlsRoot`. Kéo `WeaponSwitcher` trong WeaponHolder, `PlayerHealth` và `PlayerMovement` của Player, cùng các nút Jump/Reload/Switch/Heal/Shop/Pause vào đúng trường.
3. Thêm `MobileFireButton` lên `FireButton` (cùng object với Button); kéo `MobileCombatControls` vào field của script này. Nút bắn giữ để bắn liên tục; thả tay sẽ ngừng.
4. Thêm `TouchLookArea` lên `AimLookArea`; gán `MouseLook` trên camera. Đặt vùng này ở nửa phải/trung tâm, không phủ lên các nút. Kéo để xoay camera/ngắm; để Image trong suốt nhưng bật **Raycast Target**.
5. Trong `UIManager`, gán `Mobile Controls Root`, `Reserve Ammo Text`, `Health Packs Text`. Kéo TMP text dự trữ đạn và số health pack vào hai field tương ứng. Bật `MobileControlsRoot` lúc đầu; UIManager tự ẩn khi mở Pause/Shop/Game Over.
6. Trong `PlayerMovement`, kiểm tra `Ground Check` đã gán để joystick điều khiển Player bình thường. Đảm bảo Canvas có `GraphicRaycaster` và scene có `EventSystem` với `InputSystemUIInputModule`.
7. Tạo Button `JumpButton` trong `MobileControlsRoot`, đặt gần joystick hoặc cụm điều khiển nhưng không che `AimLookArea`. Đổi nhãn nút thành `JUMP`, gán Button component vào `MobileCombatControls > Jump Button`; field `Player Movement` kéo Player vào. Script tự đăng ký sự kiện Button nên **không cần thêm OnClick thủ công**. Nút gọi `RequestJump()` và chỉ nhảy khi `Ground Check` báo đang chạm đất; trong Editor, Space vẫn dùng để kiểm tra.

Inventory khởi tạo **90 viên đạn dự trữ** và **1 health pack**. M1911 không tiêu hao đạn khi bắn; HUD đánh dấu `(∞)`. AK74/Shotgun vẫn dùng đạn dự trữ và cần nạp lại như bình thường. Nút HEAL dùng một pack và chỉ hoạt động khi Player thiếu máu. Có thể mua thêm vật phẩm ở Armory trước khi vào game hoặc trong gameplay. Số tồn kho hiển thị trên HUD, shop và nút HEAL.

### Cài đặt âm thanh (Music/SFX)

Logic slider đã có trong `AudioSettingsUI.cs`; UI cần dựng thủ công để bạn tự thiết kế. Cài panel riêng trong cả scene `MainMenu` và `testscene` (scene là độc lập), rồi nối các nút mở Settings tương ứng:

1. Trong mỗi scene, dưới `Canvas` tạo một Panel tên `AudioSettingsPanel`, chứa đúng hai Slider tên `MusicSlider` và `SFXSlider`, cùng một Button `CloseSettingsButton`. Chỉnh giao diện/kích thước theo ý bạn; panel nên nằm cuối danh sách con Canvas để hiện phía trên menu.
2. Đặt Slider **Min Value = 0**, **Max Value = 1**, **Whole Numbers = off**. Tạo nhãn `MUSIC` và `SFX` cạnh hai thanh nếu muốn hiển thị tên; đây là nhãn, không phải điều khiển âm thanh thứ ba.
3. Thêm `AudioSettingsUI` vào một object luôn active, ví dụ `Canvas` hoặc Empty `AudioSettingsController` cùng cấp với panel. Gán `Settings Panel`, `Music Slider`, `SFX Slider` và `Close Button`. Script tự nạp giá trị đã lưu, cập nhật ngay khi kéo thanh và lưu bằng PlayerPrefs.
4. Ở scene `MainMenu`, chọn object `MainMenu` có `MainMenuController`; gán component `AudioSettingsUI` vào field tương ứng. Nút `Setting` hiện có đã được nối tới controller, không cần tạo nút thứ hai ở menu.
5. Ở scene `testscene`, duplicate/copy style nút `Setting` của menu sang `PauseMenu` và đặt nhãn `SETTING` (hoặc `AUDIO`). Trong `UIManager`, gán nút mới vào `Pause Settings Button` và component trên vào `Audio Settings UI`. Đóng popup sẽ quay lại đúng menu đang mở (Main Menu hoặc Pause Menu).
6. Trong `AudioManager`, gán `Music Source` và clip nền vào `Menu Music`/`Gameplay Music` để có nhạc nghe được. Hiện các clip music trong scene chưa được gán nên Music slider vẫn lưu mức mong muốn, nhưng chưa có nhạc để nghe thử. `SFX` áp dụng toàn cục cho âm thanh súng/zombie/hiệu ứng; Music được tách khỏi mức SFX.
7. Test: kéo Music xuống 0 thì nhạc nền tắt nhưng hiệu ứng vẫn nghe; kéo SFX xuống 0 thì hiệu ứng tắt còn nhạc vẫn nghe. Chuyển scene hoặc thoát/mở lại game để kiểm tra mức âm lượng được giữ.

### Zombie đi xuyên qua vật cản

Prefab zombie `Assets/Zombie/Prefabs/zombiechuan.prefab` đang dùng cả `NavMeshAgent` lẫn `Animator`; `Apply Root Motion` đã được bật trong khi `ZombieAI` cũng điều khiển hướng di chuyển qua agent. Hai thành phần cùng thay đổi transform có thể làm zombie lệch khỏi đường đi. Đã tắt `Apply Root Motion` trên prefab để NavMeshAgent là thành phần duy nhất điều khiển dịch chuyển. Trong `testscene`, `NavMeshSurface` vẫn đặt **Use Geometry = Render Meshes** và đang gán dữ liệu bake hiện tại. Collider vật lý của tường/xe không tự làm NavMeshAgent tìm đường vòng; nếu NavMesh xanh phủ dưới vật cản, agent vẫn có thể đi xuyên qua.

1. Chọn GameObject chứa `NavMeshSurface` trong Hierarchy. Đổi **Use Geometry** sang **Physics Colliders** để Collider của map được dùng làm hình học bake; giữ Layer Mask bao gồm Ground và các layer vật cản. Nếu prop không có Collider, thêm Collider phù hợp hoặc giữ `Render Meshes` và bảo đảm mesh được đưa vào bake.
2. Đặt props tĩnh trước khi bake. Mở cửa sổ Navigation/AI Navigation, bake lại NavMesh rồi kiểm tra ở Scene view: vùng xanh phải dừng trước tường/xe và có lối đi vòng quanh chúng. Tăng Agent Radius hoặc chỉnh Collider/bake clearance nếu đường sát vật cản quá.
3. Các chướng ngại di chuyển/đóng mở khi chơi cần `NavMeshObstacle` với **Carve** bật; Collider riêng vẫn cần để chặn va chạm vật lý.
4. Chạy thử để zombie đuổi vòng quanh một vật cản. Nếu vẫn đi xuyên, kiểm tra NavMesh màu xanh có phủ dưới vật cản không, Collider có bật không, và props có nằm trong Layer Mask/vùng thu thập của NavMeshSurface không. Re-bake sau mỗi thay đổi hình học tĩnh.

Không nên chỉ thêm Collider vào props rồi dừng ở đó: nó chặn va chạm vật lý, nhưng không sửa đường đi mà NavMeshAgent đang theo.

### H. Sửa zombie tự ngã và gắn animation/âm thanh

#### Vì sao zombie trông như tự chết?

Gameplay scene đang spawn prefab `Assets/Zombie/Prefabs/zombiechuan.prefab`, dùng `Assets/Zombie/Animations/Zombie.controller`. `ZombieHealth` chỉ gọi `Die()` khi `TakeDamage()` làm máu về 0; hiện trong code, chỉ `Gun.Shoot()` gọi `ZombieHealth.TakeDamage()`. Tuy nhiên Animator Controller hiện tại có các transition **không có condition** và bật `Has Exit Time`: trạng thái Idle tự chuyển sang Attack, Attack tự chuyển sang FallingBack rồi các state tiếp tục nối nhau. Vì vậy zombie tự chạy animation ngã dù chưa chết thật. Prefab hiện cũng chưa gán `deathSound`.

Phân biệt hai trường hợp khi test:
- Zombie ngã nhưng **không** tăng Kill/Credits: lỗi chuỗi animation tự chạy.
- Zombie biến mất và Kill/Credits tăng: `Die()` đã được gọi; xem Console tìm dòng `took ... damage` để biết lúc nào nhận sát thương. Bản APK cũ còn có thể chứa logic input cũ.

#### Sửa Animator Controller để chỉ ngã khi chết

1. Trong Project mở `Assets/Zombie/Prefabs/zombiechuan.prefab` (Prefab Mode); kiểm tra component `Animator > Controller` đang trỏ tới `Zombie.controller`.
2. Mở `Assets/Zombie/Animations/Zombie.controller` bằng double-click. Trong Animator window, xóa các transition vô điều kiện hiện có, đặc biệt các mũi tên tự nối `Z_Idle → Z_Attack → Z_FallingBack → ...`. Không xóa các Animation Clip khỏi Project.
3. Chọn parameter `IsAttacking`, đặt giá trị mặc định **false**. Giữ các parameter `Speed` (Float), `Attack` (Trigger), `Hit` (Trigger), `Die` (Trigger).
4. Đặt `Z_Idle` làm state mặc định. Chọn clip locomotion `Z_Run` hoặc `Z_Run_InPlace`, đặt **Loop Time** trong Inspector để zombie chạy lặp. Đặt `Z_Idle` lặp nếu clip import cho phép.
5. Tạo transition `Z_Idle → Z_Run` với condition `Speed Greater 0.1`; tắt **Has Exit Time**.
6. Tạo transition `Z_Run → Z_Idle` với condition `Speed Less 0.1`; tắt **Has Exit Time**.
7. Tạo `Any State → Z_Attack`, condition trigger `Attack`, tắt **Has Exit Time**, transition duration khoảng 0.05–0.1 giây. Tạo `Z_Attack → Z_Idle`, bật **Has Exit Time** (khoảng 0.9) để đòn tấn công phát hết một lần.
8. Tạo `Any State → Z_FallingBack`, condition trigger `Die`, tắt **Has Exit Time** và đặt transition duration ngắn. Chọn state `Z_FallingBack`, xóa mọi transition đi ra để zombie nằm lại sau khi chết. Chọn clip `Z_FallingBack` và tắt **Loop Time** trong Import Settings nếu đang bật.
9. Không nối transition tự động từ `Idle`, `Attack`, hoặc `Run` vào các clip `Z_FallingBack`/`Z_FallingForward`. Chỉ dùng clip ngã trong transition nhận `Die`.
10. Save Controller và prefab. Chạy gameplay mà không bắn: zombie không được ngã/chết. Sau khi bắn hạ một con: nó chuyển sang FallBack, phát âm thanh chết và biến mất sau khoảng 3 giây.

Controller hiện có parameter `Hit` nhưng chưa có clip/state phản ứng trúng đạn riêng trong các clip zombie đã liệt kê. Đã có code phát `hitSound` khi zombie nhận sát thương; muốn có animation giật khi trúng đạn thì import một clip hit reaction phù hợp, tạo state riêng và nối `Any State → HitReaction` bằng trigger `Hit`, sau đó trở về locomotion. Không nên dùng animation FallingBack làm HitReaction vì trông như zombie chết sau mỗi viên đạn.

#### Chuẩn bị file âm thanh

Trong `Assets`, tạo folder `Audio/SFX` rồi kéo các file âm thanh bạn đã chuẩn bị vào đó để Unity import. Khi mình rà project, chỉ thấy các WAV tiếng bước chân/tiếp đất của Starter Assets; chưa thấy clip tiếng súng hay tiếng zombie trong thư mục `Assets`, nên hiện chưa thể xác định tên clip để gán sẵn. Sau khi import, có thể mở từng clip trong Inspector nghe thử và đổi tên rõ ràng, ví dụ `SFX_M1911_Fire`, `SFX_AK74_Fire`, `SFX_Shotgun_Fire`, `SFX_Zombie_Hit`, `SFX_Zombie_Death`.

#### Gắn âm thanh, recoil và muzzle flash cho từng khẩu

Làm riêng cho **từng GameObject súng đang dùng** trong `WeaponHolder` (M1911, AK74, Shotgun):

1. Chọn model súng. Đảm bảo có đúng một component `Gun` và một `Audio Source` trên cùng GameObject; `Gun` hiện tìm AudioSource bằng `GetComponent<AudioSource>()`.
2. Trên AudioSource: tắt **Play On Awake** và **Loop**, đặt **Spatial Blend = 0 (2D)** để tiếng súng không nhỏ/khó nghe theo khoảng cách; đặt Volume vừa phải.
3. Trên component `Gun`, kéo clip tương ứng vào **Shoot Sound**. Gán tiếng nạp đạn vào **Reload Sound** nếu có. Mỗi súng có component Gun riêng nên có thể dùng tiếng súng khác nhau.
4. Trong Hierarchy của súng, tạo Empty con tên `MuzzlePoint`, đặt đúng tại đầu nòng và hướng trục **Z xanh** theo hướng viên đạn. Tạo child/hiệu ứng Particle System tại đó, tắt **Looping** và **Play On Awake**, cấu hình Emission Burst ngắn, rồi gán Particle System vào `Gun > Muzzle Flash`. Hiệu ứng sẽ được gọi mỗi lần bắn.
5. Muốn lóe sáng hơn, thêm Point Light nhỏ làm con ở đầu nòng, đặt màu vàng/cam, Range/Intensity vừa phải, tắt bóng (**Shadows: None**) và để component Light inactive lúc đầu. Kéo Light vào `Gun > Muzzle Flash Light`; code bật sáng ngắn rồi tự tắt. Gán thời lượng ở **Muzzle Flash Light Duration**.
6. Code `Gun` hiện có recoil transform: nếu để trống, nó giật nhẹ chính model rồi trả về vị trí/rotation ban đầu. Chỉnh **Recoil Local Position**, **Recoil Local Rotation** và **Recoil Recovery Speed** để cân từng khẩu. Nếu cần giữ root không di chuyển, tạo Empty `RecoilPivot` làm cha của mesh, chuyển `Gun` lên pivot và kéo pivot vào field **Recoil Transform**; căn model bên trong pivot trước khi Play.
7. Bấm Play, bắn vài phát: mỗi phát phải giảm đúng một viên, phát đúng âm thanh, nháy muzzle effect và model giật rồi trở lại. Điều chỉnh riêng cho M1911/AK74/Shotgun.

#### Gán tiếng bị trúng đạn và tiếng chết cho zombie

1. Mở prefab đang dùng trong spawner: `Assets/Zombie/Prefabs/zombiechuan.prefab`.
2. Trên component `ZombieHealth`, kéo clip bị thương vào **Hit Sound**, clip chết vào **Death Sound**. Code gọi Hit Sound mỗi lần `TakeDamage()` được gọi và Death Sound đúng một lần khi máu về 0.
3. Nếu có nhiều loại zombie trong `ZombieSpawner > Zombie Prefabs`, gán âm thanh cho từng prefab tương ứng; sửa prefab gốc để lần spawn kế tiếp nhận cấu hình.
4. Play Mode: bắn trúng zombie còn sống phải phát hit sound; bắn viên kết liễu phải phát death sound và animation FallBack. Kiểm tra Console: log `took ... damage` chỉ xuất hiện sau phát bắn trúng.

---

## 🐛 Gặp lỗi?

Xem ngay: **[COMMON_ERRORS.md](COMMON_ERRORS.md)**

Lỗi thường gặp:
- ✅ Input System error → Fixed!
- ✅ Animation errors → Fixed!
- Zombie không di chuyển → Bake NavMesh
- UI không hiện → Check EventSystem

---

## 📚 Full Documentation

| File | Mô tả |
|------|-------|
| **[README.md](README.md)** | 📖 Overview chính |
| **[SETUP_CHECKLIST.md](SETUP_CHECKLIST.md)** | ✅ Hướng dẫn setup module gameplay và kéo references trong Unity |
| **[COMMON_ERRORS.md](COMMON_ERRORS.md)** | 🐛 Lỗi thường gặp |
| [GAME_SETUP_GUIDE.md](GAME_SETUP_GUIDE.md) | 📖 Hướng dẫn chi tiết |
| [SCRIPT_REFERENCE.md](SCRIPT_REFERENCE.md) | 📝 API docs |
| [LAYERS_TAGS_GUIDE.md](LAYERS_TAGS_GUIDE.md) | 🏷️ Layers & Tags |
| [GAME_ARCHITECTURE.md](GAME_ARCHITECTURE.md) | 🏗️ Diagrams |
| [LEARNING_GUIDE.md](LEARNING_GUIDE.md) | 🎓 Learning resources |
| [FIX_INPUT_SYSTEM.md](FIX_INPUT_SYSTEM.md) | 🔧 Fix Input error |
| [FIX_ANIMATION_ERRORS.md](FIX_ANIMATION_ERRORS.md) | 🎬 Fix Animation |

---

## 🎮 Controls

**Android touch**
- Joystick góc dưới-trái — di chuyển
- Kéo vùng trống bên phải — xoay/ngắm
- Giữ Fire — bắn liên tục
- Reload — nạp từ reserve ammo
- Switch — đổi sang vũ khí kế tiếp
- HEAL xN — dùng một health pack, chỉ khi thiếu máu
- SHOP / Pause — mở shop / tạm dừng

**PC**
- **WASD** - Di chuyển
- **Mouse** - Nhìn xung quanh
- **Left Click** - Bắn
- **R** - Reload
- **Space** - Nhảy
- **Shift** - Chạy nhanh
- **ESC** - Pause

---

## 📊 Project Info

- **Unity Version:** 6.0 (6000.6.5f1)
- **Platform:** Android + PC
- **Scripts:** 15 files
- **Documentation:** 11 files
- **Status:** ✅ Logic menu/shop có sẵn; tự dựng giao diện trong Unity Editor

---

## 💡 Tips

1. Tự thiết kế Canvas, panel và button trong Unity; script chỉ xử lý logic
2. Đặt `MainMenu.unity` đầu tiên trong Scenes In Build
3. Gán đủ references trong Inspector
4. Mở `testscene.unity` nếu muốn chỉnh gameplay scene hiện có
5. Chỉ đổi Input Handling nếu Console báo lỗi input; project hiện dùng **Both**
6. Check Console nếu có lỗi màu đỏ

---

## 🎯 Workflow

```
Mở project → Dựng UI trong Unity → Gán references → PLAY / ARMORY → Build Android
```

**Thời gian setup UI:** tùy thiết kế; logic menu/shop đã có. Checklist hướng dẫn cấu hình các module gameplay.

---

**Good luck! 🚀**

*Last updated: 2026-10-13*
