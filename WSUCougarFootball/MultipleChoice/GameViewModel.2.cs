using MultipleChoice.Common;
using MultipleChoice.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace MultipleChoice
{
    public partial class GameViewModel : INotifyPropertyChanged
    {
        Question m_Question = null;
        List<Question> m_Questions = new List<Question>();
        Random m_Random = new Random((int)DateTime.Now.Ticks);
        Windows.UI.Xaml.Controls.MediaElement m_MediaPlayer
            = new Windows.UI.Xaml.Controls.MediaElement() { AutoPlay = true };

        public class Question
        {
            public int DesiredChoices = 4;
            public bool Correct = false;
            public bool Answered = false;
            public string Image { get; set; }
            public string Sound { get; set; }
            public string Answer { get; set; }
            public string[] Choices { get; set; }
        }

        #region Methods

        public void Start()
        {
            Start(true);

            // share score
            Windows.ApplicationModel.DataTransfer.DataTransferManager
                .GetForCurrentView().DataRequested += (s, e) =>
                {
                    var _String = string.Format(Strings.ShareHtml, this.Score, m_Questions.Count(), this.HighScore);
                    var _Html = Windows.ApplicationModel.DataTransfer.HtmlFormatHelper.CreateHtmlFormat(_String);
                    e.Request.Data.Properties.Title = Strings.ShareTitle;
                    e.Request.Data.Properties.Description = Strings.ShareTitle;
                    e.Request.Data.SetUri(new Uri(string.Format("ms-windows-store:REVIEW?PFN={0}", Strings.PackageFamilyName)));
                    e.Request.Data.SetHtmlFormat(_Html);
                };


            // privacy
            Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView().CommandsRequested += (s, e) =>
              e.Request.ApplicationCommands.Add(
                 new Windows.UI.ApplicationSettings.SettingsCommand("privacypolicy", "Privacy policy", async (o) =>
                 {
                     await Windows.System.Launcher.LaunchUriAsync(new Uri(Strings.PrivacyUrl));
                 })
              );

            // handle suspend
            App.Current.Suspending += (s, e) => { PauseCommandExecute(); };
        }

        private async void Start(bool start)
        {
            // validate settings
            System.Diagnostics.Contracts.Contract.Assert(this.BeginStory != null, "BeginStory is required");
            System.Diagnostics.Contracts.Contract.Assert(this.DecideStory != null, "DecideStory is required");
            System.Diagnostics.Contracts.Contract.Assert(this.RevealStory != null, "RevealStory is required");
            System.Diagnostics.Contracts.Contract.Assert(this.ClearStory != null, "ClearStory is required");
            System.Diagnostics.Contracts.Contract.EndContractBlock();

            // setup in app purchase (simulate when debugging)
            m_HideAdsFeature = await new InAppPurchaseHelper(HIDEADSFAETURENAME, Strings.SimulatePurchasing).Setup();
            this.HideAds = m_HideAdsFeature.IsPurchased;

            // setup data/questions
            AddQuestions();

            // clear any vars
            this.Score = 0;
            Selected = string.Empty;
            HighScore = HighScore;

            // start
            if (start)
                NextQuestion();

            // update location
            if (Strings.IncludeAdvertising)
                try
                {
                    var _Location = new Windows.Devices.Geolocation.Geolocator();
                    var _Position = await _Location.GetGeopositionAsync();
                    this.Latitude = _Position.Coordinate.Latitude;
                    this.Longitude = _Position.Coordinate.Longitude;
                }
                catch
                {
                    // this breakpoint may be reached if the user BLOCKS location services
                    System.Diagnostics.Debugger.Break();
                }
        }

        partial void AddQuestions();

        private async void NextQuestion()
        {
            Selected = string.Empty;

            var _Available = m_Questions.Where(x => !x.Answered).ToArray();
            if (!_Available.Any())
                return;
            var _NextIndex = m_Random.Next(0, (_Available.Count() - 1));
            this.m_Question = _Available[_NextIndex];
            this.Image = m_Question.Image;
            this.Answer = m_Question.Answer;

            // choices
            this.Choices.Clear();
            await Task.Delay(100);

            // use the developer-supplied choices
            if (m_Question.Choices != null)
                foreach (var item in m_Question.Choices)
                    this.Choices.Add(item);

            // use random choices from other answers
            if (!this.Choices.Any())
            {
                // all the wrong answers
                var _Answers = m_Questions
                    .Where(x => !x.Answer.Equals(m_Question.Answer))
                    .Select(x => x.Answer).Distinct().ToList();
                for (int i = 0; i < m_Question.DesiredChoices; i++)
                {
                    var _Index = m_Random.Next(0, _Answers.Count() - 1);
                    var _Answer = _Answers[_Index];
                    this.Choices.Add(_Answer);
                    _Answers.Remove(_Answer);
                }

                // an the correct answer
                {
                    m_Random = new Random((int)DateTime.Now.Ticks);
                    var _Index = m_Random.Next(0, this.Choices.Count() - 1);
                    this.Choices.Insert(_Index, m_Question.Answer);
                }
            }

            // start animation
            BeginStory.Begin();

            // update live tile (SQUARE ONLY)
            await SetTileNotification();
        }

        private async void PlayCurrentSound()
        {
            if (m_Question == null)
                return;

            // play sound (if there is one)
            m_MediaPlayer.Stop();
            if (!string.IsNullOrEmpty(m_Question.Sound))
            {
                var _Name = m_Question.Sound;
                var _Location = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var _Folder = await _Location.GetFolderAsync("Media");
                var _File = await _Folder.GetFileAsync(_Name);
                var _Stream = await _File.OpenAsync(Windows.Storage.FileAccessMode.Read);
                m_MediaPlayer.SetSource(_Stream, _File.ContentType);
            }
        }

        private async Task SetTileNotification()
        {
            Windows.Data.Xml.Dom.XmlDocument _Tile = null;
            if (string.IsNullOrEmpty(this.Image))
            {
                // a template without an image
                _Tile = Windows.UI.Notifications.TileUpdateManager
                    .GetTemplateContent(Windows.UI.Notifications.TileTemplateType.TileSquareText01);
            }
            else
            {
                // a template with an image
                _Tile = Windows.UI.Notifications.TileUpdateManager
                    .GetTemplateContent(Windows.UI.Notifications.TileTemplateType.TileSquarePeekImageAndText01);
                var _Path = await TileHelper
                    .ResizeForTile(m_Question.Image, TileHelper.TileSize.Square);
                (_Tile.GetElementsByTagName("image")[0] as Windows.Data.Xml.Dom.XmlElement)
                    .SetAttribute("src", _Path.ToString());
            }
            var _Texts = _Tile.GetElementsByTagName("text");
            _Texts[0].InnerText = this.Answer;
            _Texts[1].InnerText = Choices[1];
            _Texts[2].InnerText = Choices[2];
            _Texts[3].InnerText = Choices[3];
            var _Notification = new Windows.UI.Notifications.TileNotification(_Tile) { Tag = "no-repeat" };
            var _Updater = Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication();
            _Updater.Clear();
            _Updater.Update(_Notification);
        }

        private static void SetTileBadge(int value)
        {
            // update live tile badge
            var _Badge = Windows.UI.Notifications.BadgeUpdateManager
                .GetTemplateContent(Windows.UI.Notifications.BadgeTemplateType.BadgeNumber);
            (_Badge.SelectSingleNode("/badge") as Windows.Data.Xml.Dom.XmlElement)
                .SetAttribute("value", value.ToString());
            var badge = new Windows.UI.Notifications.BadgeNotification(_Badge);
            Windows.UI.Notifications.BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badge);
        }

        #endregion

        #region Handlers

        void Begin_Completed(object sender, object e)
        {
            // turn is starting
            if (Paused)
            {
                DecideStory.Begin();
                PauseCommandExecute();
            }
            else
            {
                PlayCurrentSound();
                DecideStory.Begin();
            }
        }

        void Decide_Completed(object sender, object e) { /* contune */ RevealStory.Begin(); }

        async void Reveal_Completed(object sender, object e)
        {
            // turn is over
            try { await CheckAnswer(); }
            catch { /* not in the foreground */ }
        }

        private async Task CheckAnswer()
        {
            // check answer
            m_Question.Answered = true;
            m_Question.Correct = ((Selected ?? string.Empty).Equals(Answer, StringComparison.CurrentCultureIgnoreCase));
            if (string.IsNullOrEmpty(Selected))
            {
                // no answer selected
                await new Windows.UI.Popups.MessageDialog(
                    string.Format(Strings.NoAnswerContent, this.HighScore),
                    Strings.NoAnswerTitle)
                {
                    Commands = { 
                            new Windows.UI.Popups.UICommand { Id = 1, Label = Strings.NoAnswerButton1, 
                                Invoked = (o) => { m_Question.Answered = false; } }, 
                            new Windows.UI.Popups.UICommand { Id = 2, Label = Strings.NoAnswerButton2,
                                Invoked = (o) => { /* do nothing */ }, }, 
                        }
                }.ShowAsync();
            }
            else if (m_Question.Correct)
            {
                // correct answer selected
                this.Score++;
                if (this.Score > this.HighScore)
                    this.HighScore = this.Score;
                await new Windows.UI.Popups.MessageDialog(
                    string.Format(Strings.CorrectAnswerContent, this.Score),
                    string.Format(Strings.CorrectAnswerTitle, this.Answer, this.Score)).ShowAsync();
            }
            else
            {
                // incorrect answer selected
                await new Windows.UI.Popups.MessageDialog(
                    string.Format(Strings.WrongAnswerContent, this.HighScore),
                    string.Format(Strings.WrongAnswerTitle, this.Answer, this.Score)).ShowAsync();
            }

            if (!this.m_Questions.Any(x => !x.Answered))
            {
                // end of game!
                await new Windows.UI.Popups.MessageDialog(
                    string.Format(Strings.GameOverContent, this.Score, this.m_Questions.Count()),
                    Strings.GameOverTitle)
                {
                    Commands = { 
                        new Windows.UI.Popups.UICommand { Id = 1, Label = Strings.GameOverButton1, 
                            Invoked = (o) => { ClearStory.Begin(); Start(false); } }, 
                        new Windows.UI.Popups.UICommand { Id = 1, Label = Strings.GameOverButton2, 
                            Invoked = (o) => { Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI(); } }, 
                    }
                }.ShowAsync();
            }
            else
            {
                ClearStory.Begin();
            }
        }

        void Clear_Completed(object sender, object e) { /* contune */ NextQuestion(); }

        #endregion

        #region Properties

        double m_Longitude = default(double);
        public double Longitude { get { return m_Longitude; } set { SetProperty(ref m_Longitude, value); } }

        double m_Latitude = default(double);
        public double Latitude { get { return m_Latitude; } set { SetProperty(ref m_Latitude, value); } }

        bool m_Paused = default(bool);
        public bool Paused { get { return m_Paused; } set { SetProperty(ref m_Paused, value); } }

        private Storyboard m_BeginStory;
        public Storyboard BeginStory
        {
            get { return m_BeginStory; }
            set { m_BeginStory = value; value.Completed += Begin_Completed; }
        }

        private Storyboard m_DecideStory;
        public Storyboard DecideStory
        {
            get { return m_DecideStory; }
            set { m_DecideStory = value; value.Completed += Decide_Completed; }
        }

        private Storyboard m_RevealStory;
        public Storyboard RevealStory
        {
            get { return m_RevealStory; }
            set { m_RevealStory = value; value.Completed += Reveal_Completed; }
        }

        private Storyboard m_ClearStory;
        public Storyboard ClearStory
        {
            get { return m_ClearStory; }
            set { m_ClearStory = value; value.Completed += Clear_Completed; }
        }

        bool m_CanBegin = false;
        public bool CanBegin { get { return m_CanBegin; } set { SetProperty(ref m_CanBegin, value); } }

        string m_Selected = default(string);
        public string Selected { get { return m_Selected; } set { SetProperty(ref m_Selected, value); } }

        int m_Score = 0;
        public int Score { get { return m_Score; } set { SetProperty(ref m_Score, value); } }

        string m_Image = default(string);
        public string Image { get { return m_Image; } set { SetProperty(ref m_Image, "ms-appx:/Media/" + value); } }

        private ObservableCollection<string> m_Choices = new ObservableCollection<string>();
        public ObservableCollection<string> Choices { get { return m_Choices; } }

        string m_Answer = default(string);
        public string Answer { get { return m_Answer; } set { SetProperty(ref m_Answer, value); } }

        double m_Remaining = default(double);
        public double Remaining { get { return m_Remaining; } set { SetProperty(ref m_Remaining, value); } }

        public int HighScore { get { return HighScoreSetting; } set { HighScoreSetting = value; RaisePropertyChanged(); } }
        private static string m_HighScoreKey = "Setting.HighScore";
        public static int HighScoreSetting
        {
            get { return StorageHelper.GetSetting(m_HighScoreKey, 0, StorageHelper.StorageStrategies.Roaming); }
            set
            {
                // persist to roaming setting
                StorageHelper.SetSetting(m_HighScoreKey, value, StorageHelper.StorageStrategies.Roaming);
                SetTileBadge(value);
            }
        }

        #endregion

        #region PurchaseHideAds DelegateCommand

        bool m_HideAds = false;
        public bool HideAds { get { return m_HideAds; } set { SetProperty(ref m_HideAds, value); } }

        const string HIDEADSFAETURENAME = "HideAds";
        InAppPurchaseHelper m_HideAdsFeature;

        DelegateCommand m_PurchaseHideAdsCommand = null;
        public DelegateCommand PurchaseHideAdsCommand
        {
            get
            {
                if (m_PurchaseHideAdsCommand != null)
                    return m_PurchaseHideAdsCommand;
                m_PurchaseHideAdsCommand = new DelegateCommand(
                    PurchaseHideAdsCommandExecute, PurchaseHideAdsCommandCanExecute);
                this.PropertyChanged += (s, e) => m_PurchaseHideAdsCommand.RaiseCanExecuteChanged();
                return m_PurchaseHideAdsCommand;
            }
        }
        async void PurchaseHideAdsCommandExecute()
        {
            PauseCommandExecute();
            await m_HideAdsFeature.Purchase();
            HideAds = m_HideAdsFeature.IsPurchased;
        }
        bool PurchaseHideAdsCommandCanExecute()
        {
            if (!Strings.IncludeAdvertising)
                return false;
            if (m_HideAdsFeature == null)
                return false;
            return !m_HideAdsFeature.IsPurchased;
        }

        #endregion

        #region Pause DelegateCommand

        DelegateCommand m_PauseCommand = null;
        public DelegateCommand PauseCommand
        {
            get
            {
                if (m_PauseCommand != null)
                    return m_PauseCommand;
                m_PauseCommand = new DelegateCommand(
                    PauseCommandExecute, PauseCommandCanExecute);
                this.PropertyChanged += (s, e) => m_PauseCommand.RaiseCanExecuteChanged();
                return m_PauseCommand;
            }
        }
        void PauseCommandExecute()
        {
            Paused = true;
            DecideStory.Pause();
            try { m_MediaPlayer.Stop(); }
            catch { }
        }
        bool PauseCommandCanExecute()
        {
            if (DecideStory == null)
                return false;
            return !Paused;
        }

        #endregion

        #region Resume DelegateCommand

        DelegateCommand m_ResumeCommand = null;
        public DelegateCommand ResumeCommand
        {
            get
            {
                if (m_ResumeCommand != null)
                    return m_ResumeCommand;
                m_ResumeCommand = new DelegateCommand(
                    ResumeCommandExecute, ResumeCommandCanExecute);
                this.PropertyChanged += (s, e) => m_ResumeCommand.RaiseCanExecuteChanged();
                return m_ResumeCommand;
            }
        }
        async void ResumeCommandExecute()
        {
            Paused = false;
            DecideStory.Resume();
            PlayCurrentSound();
        }
        bool ResumeCommandCanExecute() { return Paused == true; }

        #endregion

        #region PropertyChanged Event

        public event PropertyChangedEventHandler PropertyChanged;
        void SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] String propertyName = null)
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
