# 🐛 Common Errors & Quick Fixes

Tổng hợp tất cả lỗi thường gặp và cách fix nhanh.

---

## ⚡ Quick Fix Index

| Lỗi | Fix | File |
|-----|-----|------|
| Input System error | Đổi về Input Manager (Old) | [FIX_INPUT_SYSTEM.md](FIX_INPUT_SYSTEM.md) |
| Animation parameter errors | ✅ Đã fix trong code | [FIX_ANIMATION_ERRORS.md](FIX_ANIMATION_ERRORS.md) |
| Zombie không di chuyển | Bake NavMesh | [Xem dưới](#zombie-không-di-chuyển) |
| NullReferenceException | Check Inspector references | [Xem dưới](#nullreferenceexception) |
| UI không hiện | Check EventSystem | [Xem dưới](#ui-không-hiện) |

---

## 🔴 Critical Errors (Cần fix ngay)

### 1. ⚠️ Input System Error

```
InvalidOperationException: You are trying to read Input using the UnityEngine.Input class
```

**FIX:**
```
Edit > Project Settings > Player > Other Settings
Active Input Handling → "Input Manager (Old)"
Restart Unity
```

**Chi tiết:** [FIX_INPUT_SYSTEM.md](FIX_INPUT_SYSTEM.md)

---

### 2. ⚠️ Animation Parameter Errors

```
Parameter 'IsAttacking' does not exist
Parameter 'Speed' does not exist
```

**FIX:** ✅ **ĐÃ FIX TRONG CODE!**

Scripts đã được update để không crash khi zombie không có animation.

**Chi tiết:** [FIX_ANIMATION_ERRORS.md](FIX_ANIMATION_ERRORS.md)

---

## 🟡 Common Warnings (Không chặn game)

### 3. Zombie không di chuyển

**Triệu chứng:**
- Zombie đứng yên
- Không đuổi theo player

**Nguyên nhân & Fix:**

#### A. NavMesh chưa bake
```
Window > AI > Navigation
Tab "Bake"
Click "Bake"
→ Phải thấy vùng màu xanh trên map
```

#### B. Player không có tag "Player"
```
Chọn Player GameObject
Inspector > Tag dropdown
Chọn "Player"
```

#### C. NavMeshAgent bị disabled
```
Chọn Zombie
Inspector > NavMeshAgent
Check "Enabled" ✓
```

#### D. Zombie và Player cùng vị trí
```
Di chuyển zombie ra xa player
Zombie phải cách player > 15m để test
```

---

### 4. Không bắn được

**Triệu chứng:**
- Click chuột không có gì xảy ra
- Không có muzzle flash
- Console không có log

**Nguyên nhân & Fix:**

#### A. Camera không có tag "MainCamera"
```
Chọn MainCamera
Inspector > Tag
Chọn "MainCamera"
```

#### B. Gun script không có AudioSource
```
Chọn weapon (M1911)
Add Component > Audio Source
```

#### C. Ammo = 0
```
Chọn weapon
Inspector > Gun script
Current Ammo: 30
```

#### D. Raycast không hit
```
Console > Debug.Log("Hit: ...") có hiện không?
Nếu không → Check layer collision matrix
```

---

### 5. UI không hiện

**Triệu chứng:**
- Không thấy health bar, ammo, money
- Game chạy nhưng không có UI

**Nguyên nhân & Fix:**

#### A. Thiếu EventSystem
```
Hierarchy > tìm "EventSystem"
Nếu không có:
  UI > Event System
```

#### B. Canvas Scaler sai
```
Chọn Canvas
Canvas Scaler > UI Scale Mode
→ "Scale With Screen Size"
Reference Resolution: 1920 x 1080
```

#### C. UIManager chưa link references
```
Chọn Canvas
Inspector > UI Manager script
Check tất cả fields đã kéo UI vào chưa
```

#### D. TextMeshPro chưa import
```
Unity hỏi import TMP Essentials
→ Click "Import TMP Essentials"
```

---

### 6. NullReferenceException

```
NullReferenceException: Object reference not set to an instance of an object
```

**Dịch:** Có biến nào đó = null (chưa được gán)

**Cách tìm lỗi:**

#### Bước 1: Đọc error message
```
at Gun.Shoot() (at Assets/Script/Weapons/Gun.cs:45)
                    ↑ Lỗi ở dòng 45
```

#### Bước 2: Check dòng đó
```csharp
// Dòng 45
muzzleFlash.Play();  // ← muzzleFlash = null!
```

#### Bước 3: Check Inspector
```
Chọn GameObject có script Gun
Inspector > Gun component
Field "Muzzle Flash" → Phải kéo Particle System vào
```

**Fix:** Gán tất cả public variables trong Inspector!

---

### 7. Build Android lỗi

```
Build failed
IL2CPP error
Unable to build
```

**Fix:**

#### A. Check Android Build Support installed
```
Unity Hub > Installs
Unity 6.0 > Gear icon > Add Modules
✓ Android Build Support
  ✓ Android SDK & NDK Tools
  ✓ OpenJDK
```

#### B. Player Settings
```
Edit > Project Settings > Player

Other Settings:
  Scripting Backend: IL2CPP
  Target Architectures: ARM64 ✓
  Minimum API Level: Android 7.0 (API 24)
```

#### C. Clear build cache
```
File > Build Settings
Click "Clean"
Build lại
```

---

## 🟢 Performance Issues

### 8. Game lag/FPS thấp

**Fix:**

#### A. Giảm Quality Settings
```
Edit > Project Settings > Quality
Chọn preset "Low" hoặc "Medium"
```

#### B. Giảm số zombie spawn
```
ZombieSpawner script
Max Zombies: 5-10 (thay vì 20-30)
```

#### C. Tắt shadows
```
Directional Light
Shadow Type: No Shadows
```

---

## 🔵 Setup Issues

### 9. Scene trống, không có gì

**Fix:**
```
File > Open Scene
Chọn Assets/Scenes/SampleScene.unity
```

### 10. Scripts không compile

**Check Console:**
```
Window > General > Console
Xem lỗi compile
```

**Common issues:**
- Thiếu dấu ; ở cuối dòng
- Thiếu } đóng function
- Typo tên variable

### 11. Prefab bị mất link

```
Chọn prefab > Inspector
Nếu thấy "Missing (Script)"
→ Kéo script đúng vào
```

---

## 🎯 Prevention Tips

### ✅ Trước khi Play:

1. **Check Console** - Không có lỗi đỏ
2. **Check References** - Tất cả fields đã assign
3. **Save Scene** - Ctrl+S
4. **Save Project** - Ctrl+Shift+S

### ✅ Khi có lỗi:

1. **Đọc error message** - Nó cho biết lỗi ở đâu
2. **Google error** - 90% lỗi đã có người gặp
3. **Check Inspector** - Có field nào trống không
4. **Check Console** - Có warning nào không

### ✅ Debug tips:

```csharp
// In ra Console để debug
Debug.Log("Current health: " + currentHealth);
Debug.LogWarning("Low ammo!");
Debug.LogError("Player died!");
```

---

## 📋 Pre-flight Checklist

Trước khi chơi game, check:

- [ ] Input System đã đổi về Old
- [ ] NavMesh đã bake
- [ ] Player có tag "Player"
- [ ] Camera có tag "MainCamera"
- [ ] Zombie có tag "Enemy"
- [ ] Plane có layer "Ground"
- [ ] UIManager đã link UI elements
- [ ] Console không có lỗi đỏ

---

## 🆘 Emergency Recovery

### Nếu mọi thứ hỏng hết:

1. **Backup project** (copy folder)
2. **Close Unity**
3. **Delete Library folder** trong project
4. **Mở lại Unity** (sẽ reimport tất cả)
5. **Check lại setup**

### Nếu vẫn không chạy:

1. **Tạo scene mới**
2. **Import lại scripts**
3. **Setup lại từ đầu theo SETUP_CHECKLIST.md**

---

## 🎓 Learning from Errors

**Mỗi lỗi là một bài học!**

- NullReference → Nhớ assign references
- Input error → Nhớ check settings
- Animation error → Nhớ handle edge cases
- NavMesh error → Nhớ bake trước khi test

**Càng gặp lỗi nhiều, càng học được nhiều!** 💪

---

## 📞 Need More Help?

### Resources:
- [SETUP_CHECKLIST.md](SETUP_CHECKLIST.md) - Setup guide
- [GAME_SETUP_GUIDE.md](GAME_SETUP_GUIDE.md) - Detailed instructions
- [LAYERS_TAGS_GUIDE.md](LAYERS_TAGS_GUIDE.md) - Layers & Tags
- [FIX_INPUT_SYSTEM.md](FIX_INPUT_SYSTEM.md) - Input error
- [FIX_ANIMATION_ERRORS.md](FIX_ANIMATION_ERRORS.md) - Animation error

### Communities:
- Unity Forum
- Reddit r/Unity3D
- Discord Unity servers

---

**Remember: Every developer encounters errors. It's part of learning!** 🎮

*Last updated: 2026-10-13*
