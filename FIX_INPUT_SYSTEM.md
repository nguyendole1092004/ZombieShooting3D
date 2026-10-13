# 🔧 Fix Input System Error

## Lỗi gặp phải:

```
InvalidOperationException: You are trying to read Input using the UnityEngine.Input class, 
but you have switched active Input handling to Input System package in Player Settings.
```

## Nguyên nhân:

Unity 6 mặc định sử dụng **New Input System**, nhưng code trong project dùng **Old Input System** (`Input.GetKeyDown()`, `Input.GetButton()`).

---

## ✅ FIX - Cách 1: Đổi về Old Input System (KHUYẾN NGHỊ)

### Bước 1: Mở Player Settings
```
Edit > Project Settings > Player
```

### Bước 2: Tìm "Active Input Handling"
```
Player Settings window
  ↓
Tab "Other Settings"
  ↓
Cuộn xuống phần "Configuration"
  ↓
Tìm "Active Input Handling"
```

### Bước 3: Đổi setting
```
Active Input Handling: Input System Package (New)
  ↓ Đổi thành
Active Input Handling: Input Manager (Old)
```

**Hoặc nếu muốn support cả 2:**
```
Active Input Handling: Both
```

### Bước 4: Restart Unity
Unity sẽ hỏi restart → Click **Yes**

### ✅ DONE! Lỗi đã fix!

---

## 🔄 FIX - Cách 2: Update Code sang New Input System (Advanced)

Nếu muốn dùng New Input System, cần update tất cả scripts:

### Old Input (hiện tại):
```csharp
if (Input.GetKeyDown(KeyCode.Escape))
    PauseGame();

if (Input.GetButton("Fire1"))
    Shoot();

float mouseX = Input.GetAxis("Mouse X");
```

### New Input (cần update):
```csharp
using UnityEngine.InputSystem;

if (Keyboard.current.escapeKey.wasPressedThisFrame)
    PauseGame();

if (Mouse.current.leftButton.isPressed)
    Shoot();

float mouseX = Mouse.current.delta.x.ReadValue();
```

**⚠️ Cách này phức tạp hơn nhiều! Không khuyến nghị cho người mới.**

---

## 📋 KHUYẾN NGHỊ

👉 **Dùng Cách 1: Đổi về Input Manager (Old)**

Lý do:
- ✅ Đơn giản, chỉ cần đổi 1 setting
- ✅ Code đã viết sẵn sẽ chạy ngay
- ✅ Phù hợp cho project học tập
- ✅ Không cần update 14 scripts

---

## 🎯 Step-by-Step Fix (Chi tiết)

```
1. Edit > Project Settings
   ↓
2. Click tab "Player" (bên trái)
   ↓
3. Tab "Other Settings" (bên phải)
   ↓
4. Cuộn xuống phần "Configuration"
   ↓
5. Tìm "Active Input Handling"
   ↓
6. Dropdown → chọn "Input Manager (Old)"
   ↓
7. Unity hỏi restart → Yes
   ↓
8. Chờ Unity restart
   ↓
9. ✅ DONE!
```

---

## 🐛 Nếu vẫn lỗi sau khi fix:

### Check 1: Input Manager Settings
```
Edit > Project Settings > Input Manager
Kiểm tra có các inputs:
  - Horizontal
  - Vertical
  - Fire1
  - Jump
  - Mouse X
  - Mouse Y
```

### Check 2: Clear Console
```
Console > Clear
Play lại game
```

### Check 3: Reimport Scripts
```
Assets > Reimport All
```

---

## 📝 Updated SETUP_CHECKLIST

Thêm bước này vào Phase 1 (Project Setup):

```markdown
## 🎯 PHASE 1: PROJECT SETUP

- [ ] Mở Unity Hub, chọn Unity 6.0 (6000.6.5f1)
- [ ] Mở project "ZombieShooting"
- [ ] 🔧 FIX INPUT SYSTEM:
  - [ ] Edit > Project Settings > Player
  - [ ] Other Settings > Configuration
  - [ ] Active Input Handling → "Input Manager (Old)"
  - [ ] Restart Unity
- [ ] Mở scene `Assets/Scenes/SampleScene.unity`
```

---

## 💡 Giải thích cho người mới

### Old Input System:
```csharp
Input.GetKey(KeyCode.W)       // Simple, dễ hiểu
```

### New Input System:
```csharp
Keyboard.current.wKey.isPressed  // Phức tạp hơn, nhiều tính năng
```

**Cho game đơn giản:** Old Input System đủ rồi!  
**Cho game lớn:** New Input System tốt hơn (rebinding, multiple devices)

---

## ✅ Kết luận

**→ Dùng Cách 1: Đổi về "Input Manager (Old)"**

Chỉ mất 1 phút, fix xong là chơi được ngay! 🎮

---

*Last updated: 2026-10-12*
