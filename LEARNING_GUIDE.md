# 🎓 HƯỚNG DẪN HỌC TẬP - Zombie Shooting Game

Guide dành cho sinh viên mới học Unity và C#.

---

## 📖 Tổng quan

Project này được thiết kế để:
- ✅ Học các concepts cơ bản của Unity
- ✅ Thực hành C# scripting
- ✅ Hiểu game development workflow
- ✅ Hoàn thành đồ án môn học

**Thời gian hoàn thành:** 2-4 giờ (setup + test)

---

## 🎯 Học được gì từ project này?

### Unity Concepts (Beginner)

#### 1. **GameObjects & Components**
```
GameObject = Container rỗng
  ↓
Thêm Components = Thêm chức năng
  ↓
VD: Player + CharacterController + PlayerMovement
```

**Ví dụ thực tế:**
- Player = GameObject có CharacterController, PlayerMovement, PlayerHealth
- Zombie = GameObject có NavMeshAgent, ZombieAI, ZombieHealth

#### 2. **Prefabs**
```
Tạo 1 lần → Dùng nhiều lần
  ↓
Sửa prefab → Tất cả instances đều update
```

**Ví dụ:** 
- Tạo 1 Zombie prefab → Spawn 100 zombies giống nhau
- Sửa Zombie speed → Tất cả 100 zombies đều nhanh hơn

#### 3. **Scenes**
```
Scene = 1 level/màn chơi
  ↓
Chứa tất cả GameObjects, lights, camera
```

**Project này:** 1 scene chính (SampleScene)

#### 4. **Transform**
Mỗi GameObject có:
- Position (x, y, z)
- Rotation (x, y, z)
- Scale (x, y, z)

```csharp
transform.position = new Vector3(0, 1, 0);
transform.Rotate(0, 90, 0);
```

#### 5. **Tags & Layers**
- **Tags:** Đánh dấu để tìm kiếm (`GameObject.FindGameObjectWithTag("Player")`)
- **Layers:** Phân loại cho Physics và Raycast

Xem chi tiết: [LAYERS_TAGS_GUIDE.md](LAYERS_TAGS_GUIDE.md)

---

### C# Scripting (Beginner to Intermediate)

#### 1. **MonoBehaviour Lifecycle**

```csharp
void Start() 
{
    // Chạy 1 lần khi game bắt đầu
    currentHealth = maxHealth;
}

void Update() 
{
    // Chạy mỗi frame (~60 lần/giây)
    if (Input.GetKeyDown(KeyCode.Space))
        Jump();
}
```

**Các methods quan trọng:**
- `Start()` - Khởi tạo
- `Update()` - Game loop
- `FixedUpdate()` - Physics update (cố định 50 FPS)
- `OnDestroy()` - Cleanup

#### 2. **Variables**

```csharp
// Public - hiện trong Inspector, có thể chỉnh
public int maxHealth = 100;
public float speed = 5f;

// Private - chỉ dùng trong script
private int currentHealth;
private bool isMoving;
```

#### 3. **Input Handling**

```csharp
// Keyboard
if (Input.GetKey(KeyCode.W))
    MoveForward();

if (Input.GetKeyDown(KeyCode.Space))  // Nhấn 1 lần
    Jump();

// Mouse
if (Input.GetButton("Fire1"))  // Left click
    Shoot();

float mouseX = Input.GetAxis("Mouse X");
```

#### 4. **Raycast (Bắn súng)**

```csharp
RaycastHit hit;
if (Physics.Raycast(origin, direction, out hit, range))
{
    Debug.Log("Hit: " + hit.transform.name);
    
    // Lấy component từ object bị hit
    ZombieHealth zombie = hit.transform.GetComponent<ZombieHealth>();
    if (zombie != null)
        zombie.TakeDamage(25);
}
```

**Giải thích:**
- Bắn 1 tia vô hình từ camera
- Nếu trúng object → lấy thông tin
- Check xem có phải zombie không → gây damage

#### 5. **Coroutines (Advanced)**

```csharp
void StartReload()
{
    StartCoroutine(ReloadRoutine());
}

IEnumerator ReloadRoutine()
{
    isReloading = true;
    yield return new WaitForSeconds(1.5f);  // Đợi 1.5s
    currentAmmo = maxAmmo;
    isReloading = false;
}
```

**Dùng khi:** Cần đợi (reload, delay, animation)

#### 6. **Singleton Pattern**

```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}

// Gọi từ bất kỳ đâu:
GameManager.Instance.AddMoney(50);
```

**Tại sao:** Chỉ 1 GameManager duy nhất, dễ truy cập

---

### Unity Systems

#### 1. **NavMesh AI**

```
1. Bake NavMesh (Window > AI > Navigation > Bake)
   ↓
2. Add NavMeshAgent vào Zombie
   ↓
3. Set destination:
   agent.SetDestination(player.position);
   ↓
4. Zombie tự động tìm đường!
```

**Trong game:**
- Zombie tự động đi vòng obstacle
- Không cần code pathfinding

#### 2. **UI System (Canvas)**

```
Canvas
  ├── Text (TextMeshPro)
  ├── Image (Health bar, Crosshair)
  ├── Button (Shop, Pause)
  └── Panel (Menus)
```

**Update UI từ code:**
```csharp
healthBar.fillAmount = (float)currentHealth / maxHealth;
moneyText.text = "$" + money;
```

#### 3. **Audio System**

```csharp
AudioSource audioSource;
public AudioClip shootSound;

void Start()
{
    audioSource = GetComponent<AudioSource>();
}

void Shoot()
{
    audioSource.PlayOneShot(shootSound);
}
```

#### 4. **Physics & Collision**

```csharp
// Check có chạm đất không
bool isGrounded = Physics.CheckSphere(
    groundCheck.position,
    groundDistance,
    groundMask
);

// Layer Collision Matrix:
// Player va chạm Ground ✓
// Player va chạm Enemy ✓
// Enemy va chạm Enemy ✗ (uncheck)
```

---

## 🎓 Từng bước học

### Week 1: Setup & Player (2 giờ)
1. ✅ Học về GameObjects, Components
2. ✅ Tạo Player với CharacterController
3. ✅ Script PlayerMovement (WASD)
4. ✅ Script MouseLook (Camera FPS)
5. ✅ Test di chuyển

**Concepts học:** Transform, Input, Update loop

### Week 2: Weapons (2 giờ)
1. ✅ Học về Raycast
2. ✅ Script Gun (bắn, reload)
3. ✅ Script WeaponSwitcher
4. ✅ Test bắn vào objects

**Concepts học:** Raycast, GetComponent, Audio

### Week 3: Enemy AI (3 giờ)
1. ✅ Học về NavMesh
2. ✅ Script ZombieAI (states)
3. ✅ Script ZombieHealth
4. ✅ Test zombie đuổi theo

**Concepts học:** NavMeshAgent, State Machine, Tags

### Week 4: UI & Systems (2 giờ)
1. ✅ Học về Canvas UI
2. ✅ Script UIManager
3. ✅ Script GameManager (Singleton)
4. ✅ Test UI updates

**Concepts học:** UI Text/Image, Singleton, Events

### Week 5: Polish & Build (1 giờ)
1. ✅ Add sounds, effects
2. ✅ Balance game (difficulty)
3. ✅ Build cho Android
4. ✅ Test trên điện thoại

**Concepts học:** Build Settings, Android setup

---

## 📚 Tài nguyên học thêm

### Unity Official
- **Unity Learn:** https://learn.unity.com
  - Beginner tutorials
  - FPS Microgame
  
- **Unity Manual:** https://docs.unity3d.com/Manual/
  - Scripting API
  - Components reference

### YouTube Channels (Tiếng Anh)
- **Brackeys** - Best cho beginners
- **Code Monkey** - Clean code practices
- **Jason Weimann** - Intermediate topics

### Unity Scripting API
- https://docs.unity3d.com/ScriptReference/
- Search bất kỳ component/method nào

### Communities
- **Unity Forum:** https://forum.unity.com
- **Reddit r/Unity3D:** Hỏi đáp, showcase
- **Discord:** Unity Official Server

---

## 💡 Tips học Unity hiệu quả

### 1. **Đọc console log**
```csharp
Debug.Log("Player health: " + currentHealth);
Debug.LogWarning("Low ammo!");
Debug.LogError("Game over!");
```

Console là bạn tốt nhất khi debug!

### 2. **Inspector là king**
- Public variables hiện trong Inspector
- Có thể chỉnh realtime khi Play mode
- Nhưng nhớ: Changes trong Play mode sẽ KHÔNG save!

### 3. **Prefab workflow**
```
1. Tạo object trong scene
2. Setup đầy đủ components
3. Kéo vào Project → Thành prefab
4. Sửa prefab → Override all instances
```

### 4. **Tổ chức project**
```
Assets/
├── Scripts/
│   ├── Player/
│   ├── Enemy/
│   └── Managers/
├── Prefabs/
├── Materials/
└── Sounds/
```

### 5. **Version control**
- Dùng Git/GitHub
- Commit thường xuyên
- Backup project!

### 6. **Google is your friend**
- "Unity how to make character jump"
- "Unity raycast shoot tutorial"
- Hầu hết lỗi đều có trên StackOverflow

### 7. **Học từ lỗi**
```
NullReferenceException
  ↓
Có variable nào chưa assign?
  ↓
Check Inspector, kéo reference vào
```

---

## 🎯 Kiến thức cần thiết

### Trước khi bắt đầu:
- ✅ C# basics (variables, functions, classes)
- ✅ Biết dùng Unity Editor (cơ bản)
- ✅ Hiểu 3D coordinates (x, y, z)

### Không cần biết:
- ❌ Advanced C# (LINQ, delegates, events)
- ❌ Advanced Unity (Shader, Networking)
- ❌ 3D modeling (dùng assets có sẵn)

### Sẽ học được:
- ✅ Unity workflow
- ✅ C# scripting trong game
- ✅ Component-based architecture
- ✅ Game design patterns
- ✅ Build cho mobile

---

## 🐛 Lỗi thường gặp & Cách fix

### 1. NullReferenceException
```
Lỗi: Object reference not set to an instance
Fix: Check Inspector, có field nào chưa assign không?
```

### 2. Zombie không di chuyển
```
Lỗi: NavMesh chưa bake
Fix: Window > AI > Navigation > Bake
```

### 3. Không bắn được
```
Lỗi: Camera không có tag "MainCamera"
Fix: Chọn Camera > Tag > MainCamera
```

### 4. UI không hiện
```
Lỗi: Thiếu EventSystem
Fix: UI > EventSystem (Unity tự tạo)
```

### 5. Build Android lỗi
```
Lỗi: IL2CPP build failed
Fix: 
- Install Android Build Support
- Check JDK, SDK, NDK installed
- Scripting Backend: IL2CPP
```

---

## 📊 Đánh giá độ khó

| Feature | Độ khó | Thời gian |
|---------|--------|-----------|
| Player Movement | ⭐ Easy | 30 phút |
| Camera Control | ⭐ Easy | 15 phút |
| Weapon Shooting | ⭐⭐ Medium | 1 giờ |
| Zombie AI | ⭐⭐⭐ Hard | 2 giờ |
| UI System | ⭐⭐ Medium | 1 giờ |
| Shop System | ⭐⭐⭐ Hard | 1.5 giờ |

**Tổng estimated time:** 6-8 giờ (học + code + test)

---

## ✅ Checklist trước khi nộp đồ án

### Code Quality
- [ ] Tất cả scripts có comments
- [ ] Không có unused variables
- [ ] Không có errors trong Console
- [ ] Code format đẹp (indentation)

### Features
- [ ] Player di chuyển mượt
- [ ] Bắn súng hoạt động
- [ ] Zombie AI hoạt động
- [ ] UI update realtime
- [ ] Shop mua được đồ
- [ ] Game over khi chết

### Polish
- [ ] Có sounds (tối thiểu shoot sound)
- [ ] UI đẹp, dễ đọc
- [ ] Game balanced (không quá dễ/khó)
- [ ] Build được APK (nếu yêu cầu)

### Documentation
- [ ] README.md giải thích project
- [ ] Comments trong code
- [ ] Hướng dẫn chơi (controls)

---

## 🎓 Câu hỏi ôn tập

### Unity Basics
1. GameObject là gì? Component là gì?
2. Sự khác biệt giữa Start() và Update()?
3. Prefab dùng để làm gì?
4. Tags và Layers khác nhau như thế nào?

### C# Scripting
1. Public vs Private variable?
2. GetComponent<>() hoạt động ra sao?
3. Raycast dùng để làm gì?
4. Singleton pattern là gì?

### Game Systems
1. NavMesh AI hoạt động như thế nào?
2. Làm sao để update UI từ script?
3. Layer Collision Matrix dùng để làm gì?
4. Wave system hoạt động ra sao?

**Đáp án:** Đọc lại [SCRIPT_REFERENCE.md](SCRIPT_REFERENCE.md) và code!

---

## 🎉 Sau khi hoàn thành

### Nâng cấp game (Ideas)
- [ ] Thêm boss zombie
- [ ] Thêm nhiều maps
- [ ] Thêm grenade
- [ ] Thêm power-ups
- [ ] Multiplayer (advanced!)

### Portfolio
- [ ] Record gameplay video
- [ ] Screenshot features
- [ ] Upload lên GitHub
- [ ] Viết blog về experience

### Học tiếp
- [ ] Unity Intermediate tutorials
- [ ] Advanced C# (delegates, events)
- [ ] Shader programming
- [ ] Multiplayer networking

---

## 📞 Hỗ trợ

### Khi gặp vấn đề:
1. **Đọc error message** trong Console
2. **Google error** + "Unity"
3. **Check documentation** trong project này
4. **Hỏi trên Unity Forum** hoặc Discord
5. **Debug với Debug.Log()**

### Tài nguyên project:
- [SETUP_CHECKLIST.md](SETUP_CHECKLIST.md) - Setup guide
- [SCRIPT_REFERENCE.md](SCRIPT_REFERENCE.md) - API docs
- [LAYERS_TAGS_GUIDE.md](LAYERS_TAGS_GUIDE.md) - Layers & Tags
- [GAME_ARCHITECTURE.md](GAME_ARCHITECTURE.md) - Diagrams

---

**Good luck với việc học Unity! 🎮**

*Remember: Everyone starts somewhere. Keep learning, keep building!* 💪
