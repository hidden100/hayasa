using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using speed.Extensions;
using speed.enums;
using System.Threading;
using Android;

namespace speed
{
    [Activity(Label = "VS PC GAME")]
    public class _1x1GameActivity : Activity
    {
        private ObservableList<int> _pcCards = new ObservableList<int>();
        private ObservableList<int> _pcFiveCards = new ObservableList<int>();
        private ObservableList<int> _userCards = new ObservableList<int>();
        private ObservableList<int> _leftCardsDeck = new ObservableList<int>();
        private ObservableList<int> _rigthCardsDeck = new ObservableList<int>();
        private ObservableList<int> _allCards = new ObservableList<int>();
        private int _firstCard = int.MinValue;
        private int _secondCard = int.MinValue;
        private int _thirdCard = int.MinValue;
        private int _fouthCard = int.MinValue;
        private int _fifthCard = int.MinValue;
        private ObservableList<int> _leftCard = new ObservableList<int>();
        private ObservableList<int> _rightCard = new ObservableList<int>();
        int _LastClickedButton;
        int _PCLastSeenRightCard = int.MinValue;
        int _PCLastSeenLeftCard = int.MinValue;

        ImageButton _LeftCardDeck;
        ImageButton _RightCardDeck;
        ImageButton _LeftCard;
        ImageButton _RightCard ;
        ImageButton _FirstCard;
        ImageButton _SecondCard;
        ImageButton _ThirdCard;
        ImageButton _FourthCard;
        ImageButton _FifthCard;
        Button _MtDeck ;
        TextView _PCLevelShower;
        TextView _PCDeckShower;
        TextView _PCHandShower;

        int _PCLevel;
        PCGameStatus _status = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.theGame);
            _PCLevel = Principal.PCSpeedinMs;
            
             _LeftCardDeck = FindViewById<ImageButton>(Resource.Id.buttondeckEsquerda);
             _RightCardDeck = FindViewById<ImageButton>(Resource.Id.btnDeckDireita);
             _LeftCard = FindViewById<ImageButton>(Resource.Id.btnLeft);
             _RightCard = FindViewById<ImageButton>(Resource.Id.btnRight);
             _PCDeckShower = FindViewById<TextView>(Resource.Id.lblPCDeck);
             _PCHandShower = FindViewById<TextView>(Resource.Id.lblPCHand);

             _FirstCard = FindViewById<ImageButton>(Resource.Id.btnCarta1);
             _FirstCard.SetBackgroundColor(Android.Graphics.Color.Black);
             _SecondCard = FindViewById<ImageButton>(Resource.Id.btnCarta2);
             _SecondCard.SetBackgroundColor(Android.Graphics.Color.Black);
             _ThirdCard = FindViewById<ImageButton>(Resource.Id.btnCarta3);
             _ThirdCard.SetBackgroundColor(Android.Graphics.Color.Black);
             _FourthCard = FindViewById<ImageButton>(Resource.Id.btnCarta4);
             _FourthCard.SetBackgroundColor(Android.Graphics.Color.Black);
             _FifthCard = FindViewById<ImageButton>(Resource.Id.btnCarta5);
             _FifthCard.SetBackgroundColor(Android.Graphics.Color.Black);
             _MtDeck = FindViewById<Button>(Resource.Id.btnMyDeck);
             _PCLevelShower = FindViewById<TextView>(Resource.Id.lblPCLEVELshow);
             _PCLevelShower.Text = "PC Level: " + _PCLevel + " ms";
            //eventos de botao
            _LeftCard.Click += LeftCard_Click;
            _RightCard.Click += RightCard_Click;
            _FirstCard.Click +=FirstCard_Click;
            _SecondCard.Click += _SecondCard_Click;
            _ThirdCard.Click += _ThirdCard_Click;
            _FourthCard.Click += _FourthCard_Click;
            _FifthCard.Click += _FifthCard_Click;
            _MtDeck.Click += _MtDeck_Click;

            //eventos de lista
            _rightCard.OnChange += _rightCard_OnChange;
            _leftCard.OnChange += _leftCard_OnChange;
            _userCards.OnChange += _userCards_OnChange;
            _pcCards.OnChange += _pcCards_OnChange;
            _pcFiveCards.OnChange += _pcFiveCards_OnChange;
            
            CarregaDecks();
            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage("PCLevel = "+ _PCLevel +" ms, Are you ready?");
            callDialog.SetNeutralButton("Start", delegate
            {
                this._status = PCGameStatus.Running;
                VictoryConditionONAsync();
                TurnONRobotAsync();
                VerificadorDeImpossibilidadeAsync();
            });
            callDialog.SetNegativeButton("Cancel", delegate {
                var intent = new Intent(this, typeof(ChoosePCReactionActivity));

                StartActivity(intent);
            
            });

            // Show the alert dialog to the user and wait for response.
            callDialog.Show();
        }

        private void VerificadorDeImpossibilidadeAsync()
        {
            Thread lockverifier = new Thread(VerificaLock);
            lockverifier.Start();        
        }

        private void VerificaLock(object obj)
        {
            bool questionando = false;

            while(this._status == PCGameStatus.Running)
            {
                Thread.Sleep(2000);
                if (!questionando )
                {
                    RunOnUiThread(() =>
                    {
                        if (NaoEpossivelPCColocar() && NaoEPossivelUserColocar() && this._status == PCGameStatus.Running && !questionando)
                        {
                            var callDialog = new AlertDialog.Builder(this);
                            callDialog.SetMessage("Neither you nor the pc can set cards on the field,\n new cards will be placed.");
                            questionando = true;
                            callDialog.SetNeutralButton("Ready!", delegate
                            {
                                questionando = false;
                                ColocaNovasCartas();
                            });

                        // Show the alert dialog to the user and wait for response.
                        callDialog.Show();
                        }
                    });
                }
            }
        }

        private void ColocaNovasCartas()
        {
            if (_rigthCardsDeck.Count > 0 && _leftCardsDeck.Count > 0)
            {
                _rightCard = _rigthCardsDeck.TransferElements(1, _rightCard);
                _leftCard = _leftCardsDeck.TransferElements(1, _leftCard);
            }
            else
            {
                _rigthCardsDeck = _rightCard.TransferElements(_rightCard.Count, _rigthCardsDeck);
                _leftCardsDeck = _leftCard.TransferElements(_leftCard.Count, _leftCardsDeck);

                _rigthCardsDeck.Shuffle();

                _rightCard = _rigthCardsDeck.TransferElements(1, _rightCard);
                _leftCard = _leftCardsDeck.TransferElements(1, _leftCard);
            }
        }

        private bool NaoEPossivelUserColocar()
        {
            if(_firstCard >= 0 &&
                _secondCard >= 0 &&
                _thirdCard >= 0 &&
                _fouthCard >= 0 &&
                _fifthCard >= 0)
            {
                if(ConsegueAdicionar(_firstCard) ||
                    ConsegueAdicionar(_secondCard) ||
                    ConsegueAdicionar(_thirdCard) ||
                    ConsegueAdicionar(_fouthCard) ||
                    ConsegueAdicionar(_fifthCard))
                {
                    return false;
                }
                return true;
            }
            else if(_userCards.Count == 0)
            {
                if (ConsegueAdicionar(_firstCard) ||
                    ConsegueAdicionar(_secondCard) ||
                    ConsegueAdicionar(_thirdCard) ||
                    ConsegueAdicionar(_fouthCard) ||
                    ConsegueAdicionar(_fifthCard))
                {
                    return false;
                }
                return true;
            }

            return false;
        }

        private bool NaoEpossivelPCColocar()
        {
            
            if(_pcFiveCards.Count == 5 ||(_pcFiveCards.Count < 5 && _pcCards.Count == 0 ))
            {
                for(int i = 0; i < _pcFiveCards.Count; i++)
                {
                    
                    if((_pcFiveCards.Count != 5 && _pcCards.Count > 0) || ConsegueAdicionar(_pcFiveCards[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        private bool ConsegueAdicionar(int carta)
        {
            if(carta < 0)
            {
                return false;
            }
            if(_leftCard[_leftCard.Count - 1] == 9)
            {
                if (carta == 0 || carta == 8)
                    return true;
            }
            if (_leftCard[_leftCard.Count - 1] == 0)
            {
                if (carta == 9 || carta == 1)
                    return true;
            }
            if (_rightCard[_rightCard.Count - 1] == 9)
            {
                if (carta == 0 || carta == 8)
                    return true;
            }
            if (_rightCard[_rightCard.Count - 1] == 0)
            {
                if (carta == 9 || carta == 1)
                    return true;
            }

            if(_rightCard[_rightCard.Count - 1] == carta + 1 ||
                _rightCard[_rightCard.Count - 1] == carta - 1 ||
                _leftCard[_leftCard.Count - 1] == carta + 1 ||
                _leftCard[_leftCard.Count - 1] == carta - 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        void _pcFiveCards_OnChange(object sender, EventArgs e)
        {
            RunOnUiThread(() => {
                // manipulate UI controls
            _PCHandShower.Text = "PC Hand QTD: " + _pcFiveCards.Count.ToString();
            });
        }

        void _pcCards_OnChange(object sender, EventArgs e)
        {
            RunOnUiThread(() =>
            {
                _PCDeckShower.Text = "PC Deck QTD: " + _pcCards.Count.ToString();
            });
        }

        void _userCards_OnChange(object sender, EventArgs e)
        {
            RunOnUiThread(() =>
            {
                _MtDeck.Text = "QTD: " + _userCards.Count.ToString();
            });
        }

        void _leftCard_OnChange(object sender, EventArgs e)
        {
            if (_leftCard.Count > 0)
            {
                RunOnUiThread(() =>
                {
                    //_LeftCard.Text = _leftCard[_leftCard.Count - 1].ToString();
                    UpdateImage(ref _LeftCard, _leftCard[_leftCard.Count - 1]);
                });
            }
            else
            {
                RunOnUiThread(() =>
                {
                    //_LeftCard.Text = "X";
                    UpdateImage(ref _LeftCard, -1);              
                });
            }
        }

        private void UpdateImage(ref ImageButton botao, int number)
        {
           if(number < 0)
            {
                botao.SetImageResource(Resource.Drawable.numberX);
            }
           else if(number == 0)
            {
                botao.SetImageResource(Resource.Drawable.number0);
            }
            else if (number == 1)
            {
                botao.SetImageResource(Resource.Drawable.number1);
            }
            else if (number == 2)
            {
                botao.SetImageResource(Resource.Drawable.number2);
            }
            else if (number == 3)
            {
                botao.SetImageResource(Resource.Drawable.number3);
            }
            else if (number == 4)
            {
                botao.SetImageResource(Resource.Drawable.number4);
            }
            else if (number == 5)
            {
                botao.SetImageResource(Resource.Drawable.number5);
            }
            else if (number == 6)
            {
                botao.SetImageResource(Resource.Drawable.number6);
            }
            else if (number == 7)
            {
                botao.SetImageResource(Resource.Drawable.number7);
            }
            else if (number == 8)
            {
                botao.SetImageResource(Resource.Drawable.number8);
            }
            else if (number == 9)
            {
                botao.SetImageResource(Resource.Drawable.number9);
            }

            ImageView.ScaleType atual = botao.GetScaleType();
            botao.SetScaleType(ImageView.ScaleType.FitCenter);
        }

        void _rightCard_OnChange(object sender, EventArgs e)
        {
            if (_rightCard.Count > 0)
            {
                RunOnUiThread(() =>
                {
                    //_RightCard.Text = _rightCard[_rightCard.Count - 1].ToString();
                    UpdateImage(ref _RightCard, _rightCard[_rightCard.Count - 1]);
                });
            }
            else
            {
                RunOnUiThread(() =>
                {
                    //_RightCard.Text = "X";
                    UpdateImage(ref _RightCard, -1);
                });
            }
        }

        private void TurnONRobotAsync()
        {
            Thread robot = new Thread(PlayRobot);
            robot.Start();
        }

        private void PlayRobot()
        {
            while(this._status == PCGameStatus.Running)
            {
                

                if(_pcFiveCards.Count < 5)
                {
                    Thread.Sleep(_PCLevel);
                    if(_pcCards.Count > 0)
                    {
                      _pcFiveCards =  _pcCards.TransferElements(5 - _pcFiveCards.Count, _pcFiveCards);
                        _pcCards.OnChange += new EventHandler(_pcCards_OnChange);
                      _pcCards.IChanged();
                    }
                }

                if(_PCLastSeenLeftCard != _leftCard[_leftCard.Count - 1])
                {
                    Thread.Sleep(_PCLevel);
                }
                    _PCLastSeenLeftCard = _leftCard[_leftCard.Count - 1];
                    PCAdicionaCartaSePossivel(ref _pcFiveCards, ref _leftCard);

                if (_PCLastSeenRightCard != _rightCard[_rightCard.Count - 1])
                {
                    Thread.Sleep(_PCLevel);
                }
                    _PCLastSeenRightCard = _rightCard[_rightCard.Count - 1];
                    PCAdicionaCartaSePossivel(ref _pcFiveCards,ref _rightCard);

            }
        }

        private void PCAdicionaCartaSePossivel(ref ObservableList<int> pcFiveCards, ref ObservableList<int> monte)
        {
            for(int i = 0; i < pcFiveCards.Count; i++)
            {
                if( monte[monte.Count - 1] == 9)
                {
                    if(pcFiveCards[i] == 0 || pcFiveCards[i] == 8)
                    {
                        monte.Add(pcFiveCards[i]);
                        pcFiveCards.RemoveAt(i);
                        return;
                    }
                }
                if(monte[monte.Count - 1] == 0)
                {
                    if (pcFiveCards[i] == 9 || pcFiveCards[i] == 1)
                    {
                        monte.Add(pcFiveCards[i]);
                        pcFiveCards.RemoveAt(i);
                        return;
                    }
                }
                else if (pcFiveCards[i] == monte[monte.Count - 1] + 1 || pcFiveCards[i] == monte[monte.Count - 1] - 1)
                {
                    monte.Add(pcFiveCards[i]);
                    pcFiveCards.RemoveAt(i);
                    return;
                }
            }

            if(PCEnd() && this._status != PCGameStatus.UserFinished)
            {
                this._status = PCGameStatus.PCFinished;
            }
        }

        private bool PCEnd()
        {
            if (_pcCards.Count == 0 && _pcFiveCards.Count == 0)
            {
                return true;
            }
            return false;
        }

        private void VictoryConditionONAsync()
        {
            Thread verifyVictoryCondition = new Thread(VerifyVictoryCondition);
            verifyVictoryCondition.Start();
        }

        private void VerifyVictoryCondition()
        {
            while(this._status == PCGameStatus.Running)
            {

                if(PCEnd() && this._status != PCGameStatus.UserFinished)
                {
                    RunOnUiThread(() =>
                    {
                        this._status = PCGameStatus.PCFinished;
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("You Lost!! Noob!!");
                        callDialog.SetNeutralButton("OK, i know i am a loser..", delegate
                        {
                            var intent = new Intent(this, typeof(MainActivity));

                            StartActivity(intent);
                        });


                        // Show the alert dialog to the user and wait for response.
                        callDialog.Show();
                    });
                }
                else if(UserEnd())
                {
                    RunOnUiThread(() =>
                    {
                        this._status = PCGameStatus.UserFinished;

                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("You Won!! Someone give this man a cookie!");
                        callDialog.SetNeutralButton("OK, i´m sexy and i know it...", delegate
                        {
                            var intent = new Intent(this, typeof(MainActivity));

                            StartActivity(intent);
                        });
                        callDialog.Show();
                    });
                }

            }
        }

        void _MtDeck_Click(object sender, EventArgs e)
        {
            _userCards.OnChange += _userCards_OnChange;
            if (_userCards.Count > 0)
            {
                if (_firstCard == int.MinValue)
                {
                    _firstCard = _userCards[0];
                    //_FirstCard.Text = _firstCard.ToString();
                    UpdateImage(ref _FirstCard, _firstCard);
                    _userCards.RemoveAt(0);
                }
                else if(_secondCard == int.MinValue)
                {
                    _secondCard = _userCards[0];
                    //_SecondCard.Text = _secondCard.ToString();
                    UpdateImage(ref _SecondCard, _secondCard);
                    _userCards.RemoveAt(0);
                }
                else if (_thirdCard == int.MinValue)
                {
                    _thirdCard = _userCards[0];
                    //_ThirdCard.Text = _thirdCard.ToString();
                    UpdateImage(ref _ThirdCard, _thirdCard);
                    _userCards.RemoveAt(0);
                }
                else if (_fouthCard == int.MinValue)
                {
                    _fouthCard = _userCards[0];
                    //_FourthCard.Text = _fouthCard.ToString();
                    UpdateImage(ref _FourthCard, _fouthCard);
                    _userCards.RemoveAt(0);
                }
                else if(_fifthCard == int.MinValue)
                {
                    _fifthCard = _userCards[0];
                    //_FifthCard.Text = _fifthCard.ToString();
                    UpdateImage(ref _FifthCard, _fifthCard);
                    _userCards.RemoveAt(0);
                }
                _userCards.OnChange -= _userCards_OnChange;
                RetiraDestaque();
            }
            _LastClickedButton = Resource.Id.btnMyDeck;
        }

        void _FifthCard_Click(object sender, EventArgs e)
        {
            RetiraDestaque();
            if (_fifthCard != int.MinValue)
            {
                _LastClickedButton = Resource.Id.btnCarta5;
                _FifthCard .SetBackgroundColor(Android.Graphics.Color.Aqua);
            }
        }

        void _FourthCard_Click(object sender, EventArgs e)
        {
            RetiraDestaque();
            if (_fouthCard != int.MinValue)
            {
                _LastClickedButton = Resource.Id.btnCarta4;
                _FourthCard.SetBackgroundColor(Android.Graphics.Color.Aqua);
            }
        }

        void _ThirdCard_Click(object sender, EventArgs e)
        {
            RetiraDestaque();
            if (_thirdCard != int.MinValue)
            {
                _LastClickedButton = Resource.Id.btnCarta3;
                _ThirdCard.SetBackgroundColor(Android.Graphics.Color.Aqua);
            }
        }

        void _SecondCard_Click(object sender, EventArgs e)
        {
            RetiraDestaque();

            if(_secondCard != int.MinValue)
            {
                _LastClickedButton = Resource.Id.btnCarta2;
                _SecondCard.SetBackgroundColor(Android.Graphics.Color.Aqua);
            }
        }

        private void FirstCard_Click(object sender, EventArgs e)
        {
            RetiraDestaque();

            if(_firstCard != int.MinValue)
            {
                _LastClickedButton = Resource.Id.btnCarta1;
                _FirstCard.SetBackgroundColor(Android.Graphics.Color.Aqua); 
            }
        }

        private void RightCard_Click(object sender, EventArgs e)
        {
            RetiraDestaque();

            switch (_LastClickedButton)
            {
                case Resource.Id.btnCarta1:
                   if( AdicionaCartaSepossivel(ref _firstCard, ref _rightCard))
                        {
                        RunOnUiThread(() =>
                        {
                            //_FirstCard.Text = "X";
                            UpdateImage(ref _FirstCard, -1);
                        });
                    }
                    break;
                case Resource.Id.btnCarta2:
                    if(AdicionaCartaSepossivel(ref _secondCard, ref _rightCard))
                    {
                        RunOnUiThread(() =>
                        {
                            //_SecondCard.Text = "X";
                            UpdateImage(ref _SecondCard, -1);
                        });
                    }
                    break;
                case Resource.Id.btnCarta3:
                    if(AdicionaCartaSepossivel(ref _thirdCard, ref _rightCard))
                    {
                        RunOnUiThread(() =>
                        {
                            //_ThirdCard.Text = "X";
                            UpdateImage(ref _ThirdCard, -1);
                        });
                    }
                    break;
                case Resource.Id.btnCarta4:
                    if(AdicionaCartaSepossivel(ref _fouthCard, ref _rightCard))
                    {
                        RunOnUiThread(() =>
                        {
                            //_FourthCard.Text = "X";
                            UpdateImage(ref _FourthCard, -1);
                        });
                    }
                    break;
                case Resource.Id.btnCarta5:
                    if(AdicionaCartaSepossivel(ref _fifthCard, ref _rightCard))
                    {
                        RunOnUiThread(() =>
                        {
                            //_FifthCard.Text = "X";
                            UpdateImage(ref _FifthCard, -1);
                        });
                    }
                    break;
            }
            _LastClickedButton = Resource.Id.btnRight;
        }

        private void RetiraDestaque()
        {
            RunOnUiThread(() =>
            {
                _FirstCard.SetBackgroundColor(Android.Graphics.Color.Black);
                _SecondCard.SetBackgroundColor(Android.Graphics.Color.Black);
                _ThirdCard.SetBackgroundColor(Android.Graphics.Color.Black);
                _FourthCard.SetBackgroundColor(Android.Graphics.Color.Black);
                _FifthCard.SetBackgroundColor(Android.Graphics.Color.Black);
            });
        }

        void LeftCard_Click(object sender, EventArgs e)
        {
            RetiraDestaque();

            switch(_LastClickedButton)
            {
                case Resource.Id.btnCarta1:
                    if(AdicionaCartaSepossivel(ref _firstCard, ref _leftCard))
                    {
                        RunOnUiThread(() =>
                        {
                            //_FirstCard.Text = "X";
                            UpdateImage(ref _FirstCard, -1);
                        });
                    }
                    break;
                case Resource.Id.btnCarta2:
                   if (AdicionaCartaSepossivel(ref _secondCard, ref _leftCard))
                    {
                        RunOnUiThread(() =>
                        {
                            //_SecondCard.Text = "X";
                            UpdateImage(ref _SecondCard, -1);
                        });
                    }
                    break;
                case Resource.Id.btnCarta3:
                   if (AdicionaCartaSepossivel(ref _thirdCard, ref _leftCard))
                    {
                        RunOnUiThread(() =>
                        {
                            //_ThirdCard.Text = "X";
                            UpdateImage(ref _ThirdCard, -1);
                        });
                    }
                    break;
                case Resource.Id.btnCarta4:
                    if (AdicionaCartaSepossivel(ref _fouthCard, ref _leftCard))
                    {
                        RunOnUiThread(() =>
                        {
                            //_FourthCard.Text = "X";
                            UpdateImage(ref _FourthCard, -1);
                        });
                    }
                    break;
                case Resource.Id.btnCarta5:
                    if(AdicionaCartaSepossivel(ref _fifthCard, ref _leftCard))
                        {
                        RunOnUiThread(() =>
                        {
                            //_FifthCard.Text = "X";
                            UpdateImage(ref _FifthCard, -1);
                        });
                    }
                    break;
            }
            _LastClickedButton = Resource.Id.btnLeft;
        }

        private bool AdicionaCartaSepossivel(ref int toBePlaced, ref ObservableList<int> cardToBeCovered)
        {
            int ultimo = cardToBeCovered[cardToBeCovered.Count - 1] ; 
            int proximo = toBePlaced;
            if(ultimo == 9)
            {
                if (proximo == 8 || proximo == 0)
                {
                    cardToBeCovered.Add(proximo);
                    toBePlaced = int.MinValue;
                    return true;
                }
            }
            else if(ultimo == 0)
            {
                if(proximo == 1 || proximo == 9)
                {
                    cardToBeCovered.Add(proximo);
                    toBePlaced = int.MinValue;
                    return true;
                }
            }

            else
            {
                if(proximo == ultimo + 1 || proximo == ultimo - 1 )
                {
                    cardToBeCovered.Add(proximo);
                    toBePlaced = int.MinValue;
                    return true;
                }
            }
            return false;

            if(UserEnd() && this._status != PCGameStatus.PCFinished )
            {
                this._status = PCGameStatus.UserFinished;
            }
        }

        private bool UserEnd()
        {
            if(_userCards.Count == 0 && _firstCard == int.MinValue &&
                    _secondCard == int.MinValue && _thirdCard == int.MinValue
                    && _fouthCard == int.MinValue && _fifthCard == int.MinValue)
            {
                return true;
            }

            return false;
        }

        private void CarregaDecks()
        {
            LoadAllCards();
            LoadPCCards();
            LoadUserCards();
            LoadLeftCards();
            LoadRightCards();
        }

        private void LoadRightCards()
        {
            _rigthCardsDeck = _allCards.SubList(115, 15);
            _rightCard = _rigthCardsDeck.TransferElements(1, _rightCard);
        }

        private void LoadLeftCards()
        {
            _leftCardsDeck = _allCards.SubList(100, 15);
            _leftCard = _leftCardsDeck.TransferElements(1, _leftCard);
        }

        private void LoadUserCards()
        {
            _userCards = _allCards.SubList(50, 50);
        }

        private void LoadPCCards()
        {
            _pcCards = _allCards.SubList(0, 50);
        }

        private void LoadAllCards()
        {
            ObservableList<int> temp = new ObservableList<int>();
            int card = 0;
            for(int i = 0; i < 130; i++)
            {
                temp.Add(card);

                if (card == 9)
                {
                    card = 0;
                }
                else
                {
                    card++;
                }
            }
            _allCards = temp.Shuffle();
        }
    }
}