# SchedulEase (C# / Windows Forms)

Real-Time Classroom Availability and Room Locator System, rebuilt in C# from the maroon mobile prototype.

## How to run
1. Install the **.NET 8 SDK** (https://dotnet.microsoft.com/download) - or Visual Studio 2022 with the ".NET desktop development" workload.
2. Unzip, open a terminal in this folder, then run:
   ```
   dotnet run
   ```
   (or double-click `SchedulEase.csproj` in Visual Studio and press F5)

## Demo accounts (school accounts - no sign-up)
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

## Project layout
```
Program.cs / MainForm.cs   app start + the 390px "phone" window (Role -> Login -> Portal)
Models/                    Building, Classroom, ClassSession, User/Teacher/Student
Data/Campus.cs             rooms, sessions, sample data, edit/occupy rules
Data/AuthService.cs        school accounts + login check
Styling/                   Theme (exact prototype colors/fonts), Gfx (rounded drawing), Icons
Widgets/                   reusable controls: RoundedPanel, RoundedButton, FieldBox, PillLabel, ScrollBody, DimOverlay, ToastBanner, Ui helpers
Components/                BuildingCard, RoomCard, ActiveSessionCard, ClassCard, StatTile, RoomSheets (edit/details bottom sheets)
Pages/                     RoleSelectPage, LoginPage, DashboardPage, RoomsPage, SessionPage, ClassesPage
Shell/                     PortalShell (frame + navigation), HeaderBar, SideMenu (hamburger drawer)
```
Data is kept in memory, so changes reset when the app closes. To persist, swap `Campus` for a database/API.
