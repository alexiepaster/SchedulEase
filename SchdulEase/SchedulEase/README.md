# SchedulEase (C# / Windows Forms)
 Demo accounts (school accounts - no sign-up)
| Role    | ID           | Password        |
|---------|--------------|-----------------|
| Teacher | T-2026-041   | SchedulEase2026 |
| Student | 2025-10482   | SchedulEase2026 |

More accounts are in `Data/AuthService.cs`. Replace that list with your real school accounts later.

## What it does
- **Role select** -> Teacher or Student -> **Login** (no sign-up).
- **Dashboard**: campus overview + Building 1, 2 and 3 cards (total rooms, available / occupied / reserved).
  Tap a card or "View Details" to open the **Rooms** panel for that building.
- **Hamburger menu**: Dashboard, Rooms, Class Session (teacher) / Active Classes (student), Logout.
- **Rooms** (separate panel): filter by building, search, and (teachers) **Edit Room**.
  **Occupied rooms are locked** and cannot be edited (button disabled + checked again in `Campus.UpdateRoom`).
- **Class Session** (teacher): occupy an available room, live countdown, clock out to release the room.
- Students get a read-only view: room details sheet and a searchable list of active classes.
- Scroll with the mouse wheel / touchpad (scrollbars are hidden, like the prototype).


Shell/                     PortalShell (frame + navigation), HeaderBar, SideMenu (hamburger drawer)
```
