using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SchedulEaseWinforms.Models;
using SchedulEaseWinforms.UI;

namespace SchedulEaseWinforms
{
    public class MainForm : Form
    {
        private readonly List<Building> buildings = new();
        private readonly List<User> accounts = new();
        private readonly MaroonGradientPanel root;
        private readonly System.Windows.Forms.Timer clock;
        private User? currentUser;
        private string selectedRole = "Teacher";
        private bool signUpMode;
        private string teacherBuildingFilter = "All Buildings";
        private string studentBuildingFilter = "All Buildings";
        private string studentSearch = "";
        private Label? timerLabel;
        private TextBox? searchBox;
        private FlowLayoutPanel? portalContent;
        private Panel? bottomNav;
        private TextBox? authName, authId, authPassword;
        private Label? authSubtitle;
        private ComboBox? quickRoom, quickDuration;
        private TextBox? quickClass, quickSubject, quickSection;

        private IEnumerable<Classroom> AllRooms => buildings.SelectMany(b => b.Rooms);
        private IEnumerable<ClassSession> ActiveSessions => AllRooms
            .Where(r => r.ActiveSession != null && r.ActiveSession.CheckSessionStatus())
            .Select(r => r.ActiveSession!);

        public MainForm()
        {
            Text = "SchedulEase";
            ClientSize = new Size(400, 820);
            MinimumSize = new Size(416, 859);
            MaximumSize = new Size(416, 859);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Theme.SheetBg;
            Font = Theme.Font(9f);

            SeedData();
            root = new MaroonGradientPanel { Dock = DockStyle.Fill };
            Controls.Add(root);

            clock = new System.Windows.Forms.Timer { Interval = 1000 };
            clock.Tick += (_, _) => TickSessions();
            clock.Start();

            ShowRoleSelection();
        }

        private void SeedData()
        {
            var main = new Building { BuildingID = "BLDG-01", BuildingName = "Main Building" };
            var science = new Building { BuildingID = "BLDG-02", BuildingName = "Science & Tech Wing" };
            var rooms = new[]
            {
                new Classroom { RoomID="RM-204", RoomNumber="Room 204", BuildingName=main.BuildingName, Capacity=40, Equipment="Projector, whiteboard, speakers" },
                new Classroom { RoomID="RM-205", RoomNumber="Room 205", BuildingName=main.BuildingName, Capacity=35, Equipment="Whiteboard, ceiling fans" },
                new Classroom { RoomID="RM-206", RoomNumber="Room 206", BuildingName=main.BuildingName, Capacity=45, Equipment="Projector, whiteboard" },
                new Classroom { RoomID="RM-301", RoomNumber="Room 301", BuildingName=science.BuildingName, Capacity=40, Equipment="Projector, laboratory tables" },
                new Classroom { RoomID="RM-302", RoomNumber="Room 302", BuildingName=science.BuildingName, Capacity=32, Equipment="Desktop computers, 4K display" },
                new Classroom { RoomID="RM-303", RoomNumber="Room 303", BuildingName=science.BuildingName, Capacity=30, Equipment="Computer workstations, projector" }
            };
            main.Rooms.AddRange(rooms.Take(3));
            science.Rooms.AddRange(rooms.Skip(3));
            buildings.AddRange(new[] { main, science });

            var demoTeacher = new Teacher { IdNumber="T-2026-041", Name="Prof. Elena Reyes", Password="teacher123" };
            var demoStudent = new Student { IdNumber="2025-10482", Name="Marco Santos", Password="student123" };
            accounts.AddRange(new User[] { demoTeacher, demoStudent });

            var sample = new ClassSession
            {
                SessionID="SES-901", ClassCode="CS-301A", SubjectCode="Object-Oriented Programming",
                Section="BSCS-3A", DurationMinutes=90, StartTime=DateTime.Now.AddMinutes(-12),
                ExpectedEndTime=DateTime.Now.AddMinutes(78), TeacherName="Prof. Elena Reyes",
                AssignedRoom=rooms[0]
            };
            rooms[0].UpdateStatus("Occupied", sample);
        }

       private void ShowRoleSelection()
{
    root.Controls.Clear();
    root.BackColor = Theme.MaroonBottom;

    // 1. Rounded App Logo Emblem (matching the UML UI prototype icon)
    var logo = new RoundedPanel
    {
        Size = new Size(88, 88),
        Location = new Point(156, 78),
        CornerRadius = 24,
        FillColor = Color.FromArgb(42, 255, 255, 255),
        BorderColor = Color.FromArgb(110, 255, 255, 255)
    };
   logo.Paint += (_, e) =>
{
    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    using var pen = new Pen(Color.White, 2.5f);
    using var calPath = Theme.RoundedRect(new Rectangle(24, 26, 40, 38), 7);
    e.Graphics.DrawPath(pen, calPath);
    e.Graphics.DrawLine(pen, 24, 37, 64, 37);
    e.Graphics.DrawLine(pen, 35, 21, 35, 29);
    e.Graphics.DrawLine(pen, 53, 21, 53, 29);
    e.Graphics.DrawLine(pen, 36, 49, 42, 55);
    e.Graphics.DrawLine(pen, 42, 55, 53, 44);
};
    // 2. Crisp Unclipped Title & Subtitle (UseCompatibleTextRendering prevents DPI clipping)
    var title = new Label
    {
        Text = "SchedulEase",
        Location = new Point(10, 180),
        Size = new Size(380, 54),
        Font = Theme.Font(24f, FontStyle.Bold),
        ForeColor = Color.White,
        BackColor = Color.Transparent,
        TextAlign = ContentAlignment.MiddleCenter,
        UseCompatibleTextRendering = true
    };

    var sub = new Label
    {
        Text = "Real-Time Classroom Availability &\nRoom Locator System",
        Location = new Point(24, 236),
        Size = new Size(352, 48),
        Font = Theme.Font(10f, FontStyle.Regular),
        ForeColor = Color.FromArgb(235, 255, 242, 244),
        BackColor = Color.Transparent,
        TextAlign = ContentAlignment.TopCenter,
        UseCompatibleTextRendering = true
    };

    // 3. UML Domain Pill Badge (mirrors the UML UI prototype header)
    var umlBadge = new RoundedPanel
    {
        Size = new Size(276, 30),
        Location = new Point(62, 298),
        CornerRadius = 15,
        FillColor = Color.FromArgb(32, 255, 255, 255),
        BorderColor = Color.FromArgb(75, 255, 255, 255)
    };
    umlBadge.Controls.Add(new Label
    {
        Text = "User  •  Teacher  •  Student  •  Classroom",
        Dock = DockStyle.Fill,
        Font = Theme.Font(8f, FontStyle.Bold),
        ForeColor = Color.FromArgb(245, 255, 235, 238),
        BackColor = Color.Transparent,
        TextAlign = ContentAlignment.MiddleCenter,
        UseCompatibleTextRendering = true
    });

    // 4. Rounded Off-White Bottom Sheet
    var sheet = new RoundedPanel
    {
        Location = new Point(0, 455),
        Size = new Size(400, 365),
        CornerRadius = 28,
        TopCornersOnly = true,
        FillColor = Theme.SheetBg,
        BorderColor = Color.Transparent
    };

    sheet.Controls.Add(MakeLabel("Select your portal to continue", 24, 28, 352, 30, 13f, Theme.TextDark, true, ContentAlignment.MiddleCenter));
    sheet.Controls.Add(MakeLabel("Choose the account type you want to access", 24, 58, 352, 22, 8.5f, Theme.TextMuted, false, ContentAlignment.MiddleCenter));

    var teacher = MakeButton("I am a Teacher   →", 24, 100, 352, 56);
    teacher.Click += (_, _) => { selectedRole = "Teacher"; signUpMode = false; ShowAuth(); };

    var student = MakeButton("I am a Student   →", 24, 168, 352, 56, true);
    student.Click += (_, _) => { selectedRole = "Student"; signUpMode = false; ShowAuth(); };

    sheet.Controls.Add(teacher);
    sheet.Controls.Add(student);
    sheet.Controls.Add(MakeLabel("UM Visayan Campus  •  Computer Science", 24, 254, 352, 22, 8.5f, Theme.TextMuted, false, ContentAlignment.MiddleCenter));

    root.Controls.AddRange(new Control[] { logo, title, sub, umlBadge, sheet });
}
        private void ShowAuth()
        {
            root.Controls.Clear();
            root.BackColor=Theme.MaroonBottom;
            var back=MakeButton("←  Switch Role",20,20,118,34);
            back.NormalBg=Theme.AccentCrimson;
            back.Click+=(_,_)=>ShowRoleSelection();
            var title=MakeLabel(selectedRole+" Portal",24,80,352,40,22,Color.White,true);
            authSubtitle=MakeLabel("",24,120,352,34,9,Color.FromArgb(235,255,242,244),false);
            var card=new RoundedPanel { Location=new Point(18,190), Size=new Size(364,570), CornerRadius=22,
                FillColor=Theme.SheetBg, BorderColor=Color.Transparent };
            var loginTab=MakeButton("LOGIN",20,20,154,40, !signUpMode);
            var signupTab=MakeButton("SIGN UP",190,20,154,40, signUpMode);
            loginTab.Click+=(_,_)=>{signUpMode=false;ShowAuth();};
            signupTab.Click+=(_,_)=>{signUpMode=true;ShowAuth();};
            card.Controls.Add(loginTab); card.Controls.Add(signupTab);

            int y=signUpMode?88:102;
            if(signUpMode)
            {
                card.Controls.Add(MakeLabel(selectedRole+" Full Name",22,y,320,20,9,Theme.TextDark,true)); y+=25;
                authName=MakeTextBox("Enter your full name",22,y,320); card.Controls.Add(authName); y+=60;
            }
            else authName=null;

            card.Controls.Add(MakeLabel(selectedRole+" ID",22,y,320,20,9,Theme.TextDark,true)); y+=25;
            authId=MakeTextBox(selectedRole=="Teacher"?"Enter teacher ID":"Enter student ID",22,y,320);
            card.Controls.Add(authId); y+=60;
            card.Controls.Add(MakeLabel(signUpMode?"Create Password":"Password",22,y,320,20,9,Theme.TextDark,true)); y+=25;
            authPassword=MakeTextBox("Enter password",22,y,320,true); card.Controls.Add(authPassword); y+=68;

            var submit=MakeButton(signUpMode?"Create Account & Continue":"Login & Continue",22,y,320,48);
            submit.Click+=(_,_)=>Authenticate();
            card.Controls.Add(submit); y+=62;
            var hint=MakeLabel(signUpMode?"Already have an account? Select LOGIN above.":"Demo accounts are available below.",
                22,y,320,34,8.5f,Theme.TextMuted,false,ContentAlignment.MiddleCenter);
            card.Controls.Add(hint); y+=48;
            if(!signUpMode)
            {
                card.Controls.Add(MakeLabel("DEMO ACCESS",22,y,320,20,8,Theme.MaroonTop,true));
                card.Controls.Add(MakeLabel("Teacher: T-2026-041  /  teacher123\nStudent: 2025-10482  /  student123",
                    22,y+22,320,48,8,Theme.TextMuted,false));
            }
            if(authSubtitle!=null) authSubtitle.Text=signUpMode
                ? $"Create a new {selectedRole.ToLowerInvariant()} account."
                : $"Sign in with your {selectedRole.ToLowerInvariant()} credentials.";
            root.Controls.Add(back); root.Controls.Add(title); root.Controls.Add(authSubtitle); root.Controls.Add(card);
        }

        private void Authenticate()
        {
            var id=authId?.Text.Trim()??"";
            var password=authPassword?.Text??"";
            if(string.IsNullOrWhiteSpace(id)||string.IsNullOrWhiteSpace(password)||
                (signUpMode && string.IsNullOrWhiteSpace(authName?.Text)))
            { MessageBox.Show("Please complete all required fields.","Missing information",MessageBoxButtons.OK,MessageBoxIcon.Warning); return; }

            if(signUpMode)
            {
                if(accounts.Any(a=>a.IdNumber.Equals(id,StringComparison.OrdinalIgnoreCase)))
                { MessageBox.Show("That ID is already registered.","Account exists",MessageBoxButtons.OK,MessageBoxIcon.Warning); return; }
                User created=selectedRole=="Teacher"
                    ? new Teacher { IdNumber=id,Name=authName!.Text.Trim(),Password=password }
                    : new Student { IdNumber=id,Name=authName!.Text.Trim(),Password=password };
                accounts.Add(created); currentUser=created;
            }
            else
            {
                currentUser=accounts.FirstOrDefault(a=>
                    (selectedRole=="Teacher" ? a is Teacher : a is Student) && a.Login(id,password));
                if(currentUser==null)
                { MessageBox.Show("The ID or password is incorrect. Please try again.","Login failed",MessageBoxButtons.OK,MessageBoxIcon.Warning); return; }
            }
            ShowPortal();
        }

        private void ShowPortal()
        {
            root.Controls.Clear();
            root.BackColor=Theme.SheetBg;
            var header=new MaroonGradientPanel { Dock=DockStyle.Top, Height=164 };
            var roleName=currentUser?.Name??"User";
            var avatar=new RoundedPanel { Location=new Point(20,22), Size=new Size(48,48), CornerRadius=24,
                FillColor=Color.FromArgb(55,255,255,255), BorderColor=Color.FromArgb(100,255,255,255) };
            avatar.Controls.Add(new Label { Text=Initials(roleName), Dock=DockStyle.Fill, TextAlign=ContentAlignment.MiddleCenter,
                ForeColor=Color.White, Font=Theme.Font(12,FontStyle.Bold), BackColor=Color.Transparent });
            header.Controls.Add(avatar);
            header.Controls.Add(MakeLabel(roleName,80,22,230,26,13,Color.White,true));
            header.Controls.Add(MakeLabel(currentUser?.IdNumber??"",80,50,230,20,8.5f,Color.FromArgb(225,255,255,255),false));
            var logout=MakeButton("LOG OUT",300,28,82,30);
            logout.NormalBg=Theme.AccentCrimson; logout.CornerRadius=10;
            logout.Click+=(_,_)=>{currentUser=null;ShowRoleSelection();};
            header.Controls.Add(logout);
            header.Controls.Add(MakeLabel(currentUser is Teacher?"TEACHER PORTAL":"STUDENT PORTAL",20,104,350,24,10,Color.White,true));
            root.Controls.Add(header);

            portalContent=new FlowLayoutPanel { Location=new Point(0,164), Size=new Size(400,590),
                FlowDirection=FlowDirection.TopDown, WrapContents=false, AutoScroll=true,
                BackColor=Theme.SheetBg, Padding=new Padding(16,14,16,20) };
            root.Controls.Add(portalContent);
            BuildPortalContent();

            bottomNav=new Panel { Dock=DockStyle.Bottom, Height=62, BackColor=Color.White };
            bottomNav.Paint+=(_,e)=>{ using var p=new Pen(Theme.BorderGray); e.Graphics.DrawLine(p,0,0,bottomNav.Width,0); };
            var teacherTab=MakeNavButton("Teacher",8,8,120);
            var studentTab=MakeNavButton("Student",136,8,120);
            var switchTab=MakeNavButton("Switch Role",264,8,128);
            teacherTab.Click+=(_,_)=>SwitchPortalRole("Teacher");
            studentTab.Click+=(_,_)=>SwitchPortalRole("Student");
            switchTab.Click+=(_,_)=>{currentUser=null;ShowRoleSelection();};
            bottomNav.Controls.AddRange(new Control[]{teacherTab,studentTab,switchTab});
            root.Controls.Add(bottomNav);
            bottomNav.BringToFront();
        }

        private void SwitchPortalRole(string role)
        {
            if(role=="Teacher" && currentUser is Teacher) return;
            if(role=="Student" && currentUser is Student) return;
            selectedRole=role; signUpMode=false; ShowAuth();
        }

        private void BuildPortalContent()
        {
            if(portalContent==null) return;
            portalContent.Controls.Clear();
            if(currentUser is Teacher teacher) BuildTeacherPortal(teacher);
            else if(currentUser is Student student) BuildStudentPortal(student);
        }

        private void BuildTeacherPortal(Teacher teacher)
        {
            var active=AllRooms.FirstOrDefault(r=>r.ActiveSession?.TeacherName==teacher.Name && r.ActiveSession.CheckSessionStatus());
            AddSectionTitle("Your Active Session");
            if(active?.ActiveSession is ClassSession session)
            {
                var card=NewCard(342,150);
                card.Controls.Add(MakeLabel("●  LIVE CLASS",16,14,300,20,8,Theme.GreenText,true));
                card.Controls.Add(MakeLabel(session.SubjectCode+"  •  "+session.ClassCode,16,42,310,24,12,Theme.TextDark,true));
                card.Controls.Add(MakeLabel(session.AssignedRoom?.RoomNumber+"  |  "+session.Section,16,70,310,20,8.5f,Theme.TextMuted,false));
                timerLabel=MakeLabel(FormatTime(session.RemainingTime),16,96,180,34,19,Theme.MaroonTop,true);
                card.Controls.Add(timerLabel);
                var outBtn=MakeButton("CLOCK OUT",210,96,116,34,true);
                outBtn.Click+=(_,_)=>{teacher.ClockOut(active);timerLabel=null;BuildPortalContent();};
                card.Controls.Add(outBtn);
                portalContent!.Controls.Add(card);
            }
            else
            {
                timerLabel=null;
                var empty=NewCard(342,66);
                empty.Controls.Add(MakeLabel("No active class session. Start a session below.",14,12,312,40,9,Theme.TextMuted,false,ContentAlignment.MiddleLeft));
                portalContent!.Controls.Add(empty);
            }

            AddSectionTitle("Start Class Session");
            var start=NewCard(342,272);
            start.Controls.Add(MakeLabel("Class Code",14,12,145,18,8.5f,Theme.TextDark,true));
            quickClass=MakeTextBox("e.g. CS-301A",14,34,145);
            start.Controls.Add(quickClass);
            start.Controls.Add(MakeLabel("Subject",174,12,150,18,8.5f,Theme.TextDark,true));
            quickSubject=MakeTextBox("e.g. Programming",174,34,154);
            start.Controls.Add(quickSubject);
            start.Controls.Add(MakeLabel("Section",14,91,145,18,8.5f,Theme.TextDark,true));
            quickSection=MakeTextBox("e.g. BSCS-3A",14,113,145);
            start.Controls.Add(quickSection);
            start.Controls.Add(MakeLabel("Duration",174,91,150,18,8.5f,Theme.TextDark,true));
            quickDuration=new ComboBox { Location=new Point(174,113),Size=new Size(154,34),DropDownStyle=ComboBoxStyle.DropDownList,
                Font=Theme.Font(9),BackColor=Color.White };
            quickDuration.Items.AddRange(new object[]{"1hr","2hrs"}); quickDuration.SelectedIndex=0;
            start.Controls.Add(quickDuration);
            start.Controls.Add(MakeLabel("Available classroom",14,166,310,18,8.5f,Theme.TextDark,true));
            quickRoom=new ComboBox { Location=new Point(14,188),Size=new Size(314,34),DropDownStyle=ComboBoxStyle.DropDownList,
                Font=Theme.Font(9),BackColor=Color.White };
            RefreshRoomOptions(); start.Controls.Add(quickRoom);
            var begin=MakeButton("START CLASS SESSION",14,232,314,34);
            begin.Click+=(_,_)=>StartQuickSession(teacher);
            start.Controls.Add(begin);
            portalContent!.Controls.Add(start);

            AddSectionTitle("Classroom Directory");
            AddBuildingFilters(true);
            foreach(var room in FilterRooms(teacherBuildingFilter))
                AddRoomCard(room,true);
        }

        private void BuildStudentPortal(Student student)
        {
            AddSectionTitle("Find a Class or Classroom");
            searchBox=new TextBox { Width=340,Height=34,Font=Theme.Font(9),BorderStyle=BorderStyle.FixedSingle,
                Text=studentSearch,ForeColor=Theme.TextDark,Margin=new Padding(0,0,0,8) };
            searchBox.TextChanged+=(_,_)=>ApplyStudentSearch(searchBox);
            portalContent!.Controls.Add(searchBox);
            AddSectionTitle("Active Classes");
            var active=ActiveSessions.ToList();
            var matching=active.Where(s=>MatchesSession(s,studentSearch)).ToList();
            if(matching.Count==0)
            {
                var no=NewCard(342,58);
                no.Controls.Add(MakeLabel(string.IsNullOrWhiteSpace(studentSearch)?"No active classes at the moment.":"No matching active class found.",
                    14,12,312,34,9,Theme.TextMuted,false,ContentAlignment.MiddleLeft));
                portalContent.Controls.Add(no);
            }
            else foreach(var session in matching)
            {
                var card=NewCard(342,116);
                card.Controls.Add(MakeLabel("●  ACTIVE NOW",14,10,300,18,8,Theme.GreenText,true));
                card.Controls.Add(MakeLabel(session.SubjectCode+"  •  "+session.ClassCode,14,32,312,22,11,Theme.TextDark,true));
                card.Controls.Add(MakeLabel(session.AssignedRoom?.RoomNumber+"  |  "+session.AssignedRoom?.BuildingName,14,58,312,18,8.5f,Theme.TextMuted,false));
                card.Controls.Add(MakeLabel("Teacher: "+session.TeacherName+"     Section: "+session.Section,14,80,312,18,8,Theme.TextMuted,false));
                portalContent.Controls.Add(card);
            }
            AddSectionTitle("Classroom Directory");
            AddBuildingFilters(false);
            foreach(var room in FilterRooms(studentBuildingFilter).Where(r=>MatchesRoom(r,studentSearch)))
                AddRoomCard(room,false);
        }

        private void ApplyStudentSearch(TextBox source)
        {
            studentSearch = source.Text;
            int caret = source.SelectionStart;
            BuildPortalContent();
            if (searchBox != null)
            {
                searchBox.Focus();
                searchBox.SelectionStart = Math.Min(caret, searchBox.TextLength);
            }
        }

        private bool MatchesSession(ClassSession s,string query) =>
            string.IsNullOrWhiteSpace(query) ||
            new[]{s.ClassCode,s.SubjectCode,s.Section,s.TeacherName,s.AssignedRoom?.RoomNumber??"",s.AssignedRoom?.BuildingName??""}
                .Any(v=>v.Contains(query,StringComparison.OrdinalIgnoreCase));

        private bool MatchesRoom(Classroom r,string query) =>
            string.IsNullOrWhiteSpace(query) ||
            new[]{r.RoomNumber,r.BuildingName,r.Status,r.ActiveSession?.ClassCode??"",r.ActiveSession?.SubjectCode??""}
                .Any(v=>v.Contains(query,StringComparison.OrdinalIgnoreCase));

        private IEnumerable<Classroom> FilterRooms(string filter) =>
            AllRooms.Where(r=>filter=="All Buildings"||r.BuildingName==filter);

        private void AddBuildingFilters(bool teacher)
        {
            var row=new FlowLayoutPanel { Width=342,Height=40,AutoSize=false,WrapContents=false,
                FlowDirection=FlowDirection.LeftToRight,BackColor=Theme.SheetBg,Margin=new Padding(0,0,0,8) };
            var current=teacher?teacherBuildingFilter:studentBuildingFilter;
            foreach(var name in new[]{"All Buildings"}.Concat(buildings.Select(b=>b.BuildingName)))
            {
                var b=MakeButton(name,0,0,name=="All Buildings"?100:120,32,name==current);
                b.Font=Theme.Font(7.5f,FontStyle.Bold);
                b.Click+=(_,_)=>{if(teacher)teacherBuildingFilter=name;else studentBuildingFilter=name;BuildPortalContent();};
                row.Controls.Add(b);
            }
            portalContent!.Controls.Add(row);
        }

        private void AddRoomCard(Classroom room,bool teacherView)
        {
            bool available=room.CheckAvailability();
            var card=NewCard(342,available?112:132);
            card.Controls.Add(MakeLabel(room.RoomNumber,14,12,190,22,11,Theme.TextDark,true));
            var status=MakeLabel(available?"AVAILABLE":"OCCUPIED",214,12,112,22,7.5f,available?Theme.GreenText:Theme.AmberText,true,ContentAlignment.MiddleRight);
            card.Controls.Add(status);
            card.Controls.Add(MakeLabel(room.BuildingName+"  •  Capacity "+room.Capacity,14,39,310,18,8,Theme.TextMuted,false));
            if(!available && room.ActiveSession!=null)
                card.Controls.Add(MakeLabel(room.ActiveSession.SubjectCode+"  •  "+room.ActiveSession.TeacherName,14,60,310,18,8,Theme.TextMuted,false));
            var details=MakeButton("VIEW DETAILS",14,available?70:91,142,30,true);
            details.Click+=(_,_)=>ShowRoomDetails(room,teacherView);
            card.Controls.Add(details);
            if(teacherView && available)
            {
                var occupy=MakeButton("OCCUPY ROOM",166,70,160,30);
                occupy.Click+=(_,_)=>ShowOccupyDialog(room);
                card.Controls.Add(occupy);
            }
            portalContent!.Controls.Add(card);
        }

        private void AddSectionTitle(string title)
        {
            portalContent!.Controls.Add(MakeLabel(title,0,0,342,26,11,Theme.TextDark,true));
        }

        private void StartQuickSession(Teacher teacher)
        {
            var room=quickRoom?.SelectedItem as Classroom;
            if(room==null || quickClass==null || quickSubject==null || quickSection==null ||
                string.IsNullOrWhiteSpace(quickClass.Text)||string.IsNullOrWhiteSpace(quickSubject.Text)||string.IsNullOrWhiteSpace(quickSection.Text))
            { MessageBox.Show("Complete the class details and select an available room.","Missing information",MessageBoxButtons.OK,MessageBoxIcon.Warning);return; }
            try
            {
                int duration=quickDuration?.SelectedIndex==1?120:60;
                teacher.OccupyRoom(room,quickClass.Text,quickSubject.Text,quickSection.Text,duration);
                BuildPortalContent();
            }
            catch(Exception ex){MessageBox.Show(ex.Message,"Unable to start session",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
        }

        private void RefreshRoomOptions()
        {
            if(quickRoom==null) return;
            quickRoom.Items.Clear();
            foreach(var room in AllRooms.Where(r=>r.CheckAvailability())) quickRoom.Items.Add(room);
            quickRoom.DisplayMember="RoomNumber";
            if(quickRoom.Items.Count>0) quickRoom.SelectedIndex=0;
        }

        private void ShowOccupyDialog(Classroom room)
        {
            if(currentUser is not Teacher teacher) return;
            using var dialog=new Form { Text="Occupy Classroom",ClientSize=new Size(330,410),
                FormBorderStyle=FormBorderStyle.FixedDialog,StartPosition=FormStartPosition.CenterParent,
                MaximizeBox=false,MinimizeBox=false,BackColor=Theme.SheetBg };
            dialog.Controls.Add(MakeLabel("Start a Class Session",20,18,290,28,15,Theme.TextDark,true));
            dialog.Controls.Add(MakeLabel(room.RoomNumber+"  •  "+room.BuildingName,20,52,290,22,9,Theme.TextMuted,false));
            var c=MakeTextBox("Class code",20,100,290); var s=MakeTextBox("Subject",20,164,290); var sec=MakeTextBox("Section",20,228,290);
            dialog.Controls.Add(MakeLabel("Class Code",20,80,290,18,8.5f,Theme.TextDark,true)); dialog.Controls.Add(c);
            dialog.Controls.Add(MakeLabel("Subject",20,144,290,18,8.5f,Theme.TextDark,true)); dialog.Controls.Add(s);
            dialog.Controls.Add(MakeLabel("Section",20,208,290,18,8.5f,Theme.TextDark,true)); dialog.Controls.Add(sec);
            dialog.Controls.Add(MakeLabel("Duration",20,272,290,18,8.5f,Theme.TextDark,true));
            var duration=new ComboBox { Location=new Point(20,294),Size=new Size(290,34),DropDownStyle=ComboBoxStyle.DropDownList,Font=Theme.Font(9) };
            duration.Items.AddRange(new object[]{"1hr","2hrs"});duration.SelectedIndex=0;dialog.Controls.Add(duration);
            var start=MakeButton("START SESSION",20,350,290,42);
            start.Click+=(_,_)=>{
                if(string.IsNullOrWhiteSpace(c.Text)||string.IsNullOrWhiteSpace(s.Text)||string.IsNullOrWhiteSpace(sec.Text))
                {MessageBox.Show(dialog,"Please complete all fields.");return;}
                try{teacher.OccupyRoom(room,c.Text,s.Text,sec.Text,duration.SelectedIndex==1?120:60);dialog.DialogResult=DialogResult.OK;dialog.Close();}
                catch(Exception ex){MessageBox.Show(dialog,ex.Message);}
            };
            dialog.Controls.Add(start);
            if(dialog.ShowDialog(this)==DialogResult.OK) BuildPortalContent();
        }

        private void ShowRoomDetails(Classroom room,bool teacherView)
        {
            using var dialog=new Form { Text="Classroom Details",ClientSize=new Size(340,330),
                FormBorderStyle=FormBorderStyle.FixedDialog,StartPosition=FormStartPosition.CenterParent,
                MaximizeBox=false,MinimizeBox=false,BackColor=Theme.SheetBg };
            dialog.Controls.Add(MakeLabel(room.RoomNumber,20,20,300,30,17,Theme.TextDark,true));
            dialog.Controls.Add(MakeLabel(room.BuildingName,20,52,300,22,9,Theme.TextMuted,false));
            bool available=room.CheckAvailability();
            dialog.Controls.Add(MakeLabel(available?"● AVAILABLE":"● OCCUPIED",20,90,300,24,9,
                available?Theme.GreenText:Theme.AmberText,true));
            dialog.Controls.Add(MakeLabel("Capacity: "+room.Capacity+" learners",20,128,300,22,9,Theme.TextDark,false));
            dialog.Controls.Add(MakeLabel("Equipment: "+room.Equipment,20,156,300,42,9,Theme.TextMuted,false));
            if(room.ActiveSession!=null)
            {
                dialog.Controls.Add(MakeLabel("Current class",20,206,300,20,9,Theme.TextDark,true));
                dialog.Controls.Add(MakeLabel(room.ActiveSession.SubjectCode+" ("+room.ActiveSession.ClassCode+")\n"+room.ActiveSession.TeacherName+"  •  "+room.ActiveSession.Section,
                    20,230,300,44,8.5f,Theme.TextMuted,false));
            }
            var close=MakeButton("CLOSE",20,282,300,34,true);close.Click+=(_,_)=>dialog.Close();dialog.Controls.Add(close);
        }

        private void TickSessions()
        {
            bool changed=false;
            foreach(var room in AllRooms.Where(r=>r.ActiveSession!=null).ToList())
            {
                if(!room.ActiveSession!.CheckSessionStatus())
                { room.UpdateStatus("Available",null); changed=true; }
            }
            if(timerLabel!=null)
            {
                var session=AllRooms.Select(r=>r.ActiveSession).FirstOrDefault(s=>s!=null&&s.TeacherName==currentUser?.Name);
                timerLabel.Text=session==null?"00:00:00":FormatTime(session.RemainingTime);
            }
            if(changed && portalContent!=null) BuildPortalContent();
        }

        private static string FormatTime(TimeSpan time)
        {
            if(time<TimeSpan.Zero) time=TimeSpan.Zero;
            return $"{(int)time.TotalHours:00}:{time.Minutes:00}:{time.Seconds:00}";
        }

        private static string Initials(string name)
        {
            var parts=name.Split(' ',StringSplitOptions.RemoveEmptyEntries);
            return parts.Length==1?parts[0][..Math.Min(2,parts[0].Length)].ToUpperInvariant():
                (parts[0][0].ToString()+parts[^1][0]).ToUpperInvariant();
        }

        private static Label MakeLabel(string text,int x,int y,int w,int h,float size,Color color,bool bold,
            ContentAlignment align=ContentAlignment.MiddleLeft)
        {
            return new Label { Text=text,Location=new Point(x,y),Size=new Size(w,h),Font=Theme.Font(size,bold?FontStyle.Bold:FontStyle.Regular),
                ForeColor=color,BackColor=Color.Transparent,TextAlign=align,AutoEllipsis=true };
        }

        private static RoundedButton MakeButton(string text,int x,int y,int w,int h,bool secondary=false)
        {
            return new RoundedButton { Text=text,Location=new Point(x,y),Size=new Size(w,h),CornerRadius=11,
                NormalBg=secondary?Color.White:Theme.MaroonTop,HoverBg=secondary?Theme.BlushTint:Theme.AccentCrimson,
                ForeColor=secondary?Theme.MaroonTop:Color.White,OutlineColor=secondary?Theme.BlushBorder:Color.Transparent };
        }

        private static RoundedButton MakeNavButton(string text,int x,int y,int w)
        {
            return new RoundedButton { Text=text,Location=new Point(x,y),Size=new Size(w,44),CornerRadius=10,
                Font=Theme.Font(8,FontStyle.Bold),NormalBg=Theme.BlushTint,HoverBg=Theme.BlushBorder,
                ForeColor=Theme.MaroonTop,OutlineColor=Theme.BlushBorder };
        }

        private static RoundedPanel NewCard(int height,int width)
        {
            // width is retained as a named argument for readability at call sites.
            return new RoundedPanel { Size=new Size(height,width),CornerRadius=14,FillColor=Color.White,
                BorderColor=Theme.BorderGray,Margin=new Padding(0,0,0,10),Padding=new Padding(0) };
        }

        private static TextBox MakeTextBox(string placeholder,int x,int y,int width,bool password=false)
        {
            var box=new TextBox { Location=new Point(x,y),Size=new Size(width,34),Font=Theme.Font(9),
                BorderStyle=BorderStyle.FixedSingle,BackColor=Color.White,ForeColor=Theme.TextDark,
                PlaceholderText=placeholder,UseSystemPasswordChar=password };
            return box;
        }
    }
}
