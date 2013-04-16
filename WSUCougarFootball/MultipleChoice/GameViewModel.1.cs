using MultipleChoice.Common;
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
        public GameViewModel()
        {
            // this is the designtime data

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Image = "Chicken.png";
                Answer = "Sample Answer";
                Choices.Add("Sample Choice");
                Choices.Add("Sample Choice");
                Choices.Add("Sample Choice");
                Choices.Add("Sample Choice");
                Choices.Add("Sample Choice");
                Selected = Choices.First();
            }
        }

        partial void AddQuestions()
        {
            // this is the runtime data

            this.m_Questions.Clear();
            this.m_Questions.AddRange(new Question[] 
            { 
                // should be a minimum of 3, no real max though
                new Question { Image = "Chicken.png", Sound = "Chicken.wav", Answer = "Chicken" },
                new Question { Image = "Cow.png", Sound = "Cow.wav", Answer = "Cow" },
                new Question { Image = "Goat.png", Sound = "Goat.wav", Answer = "Goat" },
                new Question { Image = "Horse.png", Sound = "Horse.wav", Answer = "Horse" },
                new Question { Image = "Pig.png", Sound = "Pig.wav", Answer = "Pig" },
                new Question { Image = "Rooster.png", Sound = "Rooster.wav", Answer = "Rooster" },
                new Question { Image = "Sheep.png", Sound = "Sheep.wav", Answer = "Sheep" },
            });
        }

        public class Strings
        {
            public const string PrivacyUrl = "http://blog.jerrynixon.com/p/liquid47.html";

            public const string WelcomeTitle = "Ready to Play?";
            public const string WelcomeContent = "Do you know your farm animals?";
            public const string WelcomeButton1 = "Start";

            public const string NoAnswerTitle = "Um, are you there? Be sure and pick an answer!";
            public const string NoAnswerContent = "Your high score is {0}. Keep going!";
            public const string NoAnswerButton1 = "Try Again";
            public const string NoAnswerButton2 = "It's Okay";

            public const string CorrectAnswerTitle = "Yes! That's a {0}! Your score is now {1}";
            public const string CorrectAnswerContent = "Your high score is {0}. Keep going!";

            public const string WrongAnswerTitle = "Sorry! That's a {0}! Your score is {1}";
            public const string WrongAnswerContent = "Your high score is {0}. Keep going!";

            public const string GameOverTitle = "Game Over!";
            public const string GameOverContent = "Your score was {0} out of {1}";
            public const string GameOverButton1 = "Play Again";
            public const string GameOverButton2 = "Share Score";

            // 0 == number correct this session
            // 1 == total possible this session
            // 2 == high score
            public const string ShareHtml = "I am playing Farm Animals Quiz!<p>I've answered {0} out of {1}.<p>My high score is {2}!<p>You should try it!";
            public const string ShareTitle = "Farm Animals Quiz";

            public const string SettingsAboutButton = "About";
            public const string SettingsPrivacyButton = "Privacy";

            // set to false to hide ads everywhere hardcoded
            public static bool IncludeAdvertising = true; // in general this should be left to true
            public static bool SimulatePurchasing = System.Diagnostics.Debugger.IsAttached;

            // get your ad values from http://pubcenter.microsoft.com
            public const string AdApplicationId = "43da88f7-2b36-46f3-81dd-0b043193e1c6";
            public const string AdUnitId = "10056298";

            // get your package name from your manifest AFTER you associate it with the Store
            public const string PackageFamilyName = "35527Liquid47.FarmAnimalsQuiz_y1fzmhwcb5c58"; // AppxManifest/Packaging(tab)
        }
    }
}
