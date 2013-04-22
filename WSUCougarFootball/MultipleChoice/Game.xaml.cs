using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MultipleChoice
{
    public sealed partial class Game : MultipleChoice.Common.LayoutAwarePage
    {
        public Game()
        {
            this.InitializeComponent();

            // animate the grass
            BounceTheGrass.Begin();

            // start the game
            Loaded += async (s, e) =>
            {
                AnimateSun.To = Window.Current.Bounds.Width + Sun.Width;

                var _ViewModel = this.DataContext as GameViewModel;
                _ViewModel.BeginStory = this.BeginStory;
                _ViewModel.DecideStory = this.DecideStory;
                _ViewModel.RevealStory = this.RevealStory;
                _ViewModel.ClearStory = this.ClearStory;

                // end of game!
                var _Dialog = new Windows.UI.Popups.MessageDialog(
                    MultipleChoice.GameViewModel.Strings.WelcomeContent,
                    MultipleChoice.GameViewModel.Strings.WelcomeTitle)
                {
                    Commands = { new Windows.UI.Popups.UICommand { Id = 1, 
                        Label =  MultipleChoice.GameViewModel.Strings.WelcomeButton1, 
                        Invoked = (o) => _ViewModel.Start() }, }
                };
                await _Dialog.ShowAsync();

                if (!MultipleChoice.GameViewModel.Strings.IncludeAdvertising)
                {
                    MyAdControl.Visibility = Visibility.Collapsed;
                    MyPurchaseButton.Visibility = Visibility.Collapsed;
                }
            };
        }

        // snap view
        private void Storyboard_Completed_2(object sender, object e)
        {
            var _ViewModel = this.DataContext as GameViewModel;
            _ViewModel.PauseCommand.Execute(this);
        }
    }
}
