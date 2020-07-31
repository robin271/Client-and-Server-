using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GamePlay
{
    public class FieldActions : MonoBehaviour
    {
        #region Declarations
        public Transform Tiles;
        public Text EventText;

        Dictionary<string, GameObject> CanvObjs;
  
        
        public static int InfectedPeople=5;
        public int AllPeople =100;
        public int NewInfections = 5;

        private string[] _Event = new string[11];
        public int[] Bought = new int[40];
        public int[] Level1 = new int[40];
        public int[] Level2 = new int[40];

          
        public int Chip = 0;

        private bool GameOver;
        protected int Pos;
        public static int Money=500;
        private int _opos;
        #endregion
        private void Start()
        {
            #region Assigning CanvObjs
            CanvObjs = new Dictionary<string, GameObject>();
            CanvObjs.Add("HealthCenter", GetComponentInChildren<RectTransform>().GetChild(0).gameObject);
            CanvObjs.Add("ResearchLab",GetComponentInChildren<RectTransform>().GetChild(1).gameObject);
            CanvObjs.Add("EventCard",GetComponentInChildren<RectTransform>().GetChild(2).gameObject);
            CanvObjs.Add("PercentageInfected", GetComponentInChildren<RectTransform>().GetChild(3).gameObject);
            CanvObjs.Add("Money", GetComponentInChildren<RectTransform>().GetChild(4).gameObject);
            CanvObjs.Add("Chips", GetComponentInChildren<RectTransform>().GetChild(5).gameObject);
            CanvObjs.Add("primEcon", GetComponentInChildren<RectTransform>().GetChild(6).gameObject);
            CanvObjs.Add("secEcon", GetComponentInChildren<RectTransform>().GetChild(7).gameObject);
            CanvObjs.Add("terEcon", GetComponentInChildren<RectTransform>().GetChild(8).gameObject);
            #endregion
            #region Assigning Events
            _Event[0] = "The Virus mutated; your country has to start working on the research again. It costs you 3 Research Chips";
            _Event[1] = "The Virus now also spreads through rats. Everytime you land on a Infection Field 2% more of your people will be infected";
            _Event[2] = "Your economy was because of the tons of employees working at home.  It costs you $50 million to keep them alive.";
            _Event[3] = "There is an earthquake going on in your country. You support the people with $20 million of your countries money";
            _Event[4] = "Your people got the flu and a lot of people are staying at home. Your economy is weakened by this and you pay them §10 million dollars";
            _Event[5] = "Tons of people are protesting and not wearing masks 4% of your total population is getting infected";
            _Event[6] = $"You made a good speech and most of the people are staying home. Less people are getting infected and the infection rate goes down to {InfectedPeople - 4}%";
            _Event[7] = "A lot of people started using disinfectant spray. 4% less people were getting sick.";
            _Event[8] = "Teenagers don't care about washing their hands anymore and 40% of them got sick.";
            _Event[9] = "Your Virologist found out an important thing about the cell structure from the virus. You received 3 Chips";
            _Event[10] = "People started taking test and are staying home if they have it. That saves 2% of your population";
            #endregion
        }
        void Update()
        {
            if (ownVar.finishedLevels==2){Pos = ownVar.OnField[ownVar.Turn-1];}
            //TODO make a better check
            if (Pos != _opos)
            {
                #region Normalize
                    CanvObjs["HealthCenter"].SetActive(false);
                    CanvObjs["ResearchLab"].SetActive(false);
                    CanvObjs["primEcon"].SetActive(false);
                    CanvObjs["secEcon"].SetActive(false);
                    CanvObjs["primEcon"].SetActive(false);
                #endregion
                switch (Pos)
                {
                    case 1:
                        InfectedPeople += NewInfections;
                        break;
                    case 6:
                        InfectedPeople += NewInfections;
                        break;
                    case 14:
                        InfectedPeople += NewInfections;
                        break;
                    case 24:
                        InfectedPeople += NewInfections;
                        break;
                    case 30:
                        InfectedPeople += NewInfections;
                        break;
                    case 39:
                        InfectedPeople += NewInfections;
                        break;
                    case 3:
                        researchLabField();
                        break;
                    case 8:
                        researchLabField();
                        break;
                    case 12:
                        researchLabField();
                        break;
                    case 17:
                        researchLabField();
                        break;
                    case 20:
                        researchLabField();
                        break;
                    case 27:
                        researchLabField();
                        break;
                    case 34:
                        researchLabField();
                        break;
                    case 38:
                        researchLabField();
                        break;
                    case 2:
                        HealthCenterField();
                        break;
                    case 9:
                        HealthCenterField();
                        break;
                    case 13:
                        HealthCenterField();
                        break;
                    case 18:
                        HealthCenterField();
                        break;
                    case 22:
                        HealthCenterField();
                        break;
                    case 29:
                        HealthCenterField();
                        break;
                    case 32:
                        HealthCenterField();
                        break;
                    case 36:
                        HealthCenterField();
                        break;
                    case 4:
                        secEcon();
                        break;
                    case 5:
                        primEcon();
                        break;
                    case 7:
                        terEcon();
                        break;
                    case 10:
                        secEcon();
                        break;
                    case 15:
                        primEcon();
                        break;
                    case 16:
                        terEcon();
                        break;
                    case 19:
                        secEcon();
                        break;
                    case 21:
                        primEcon();
                        break;
                    case 23:
                        terEcon();
                        break;
                    case 25:
                        secEcon();
                        break;
                    case 26:
                        terEcon();
                        break;
                    case 28:
                        terEcon();
                        break;
                    case 31:
                        primEcon();
                        break;
                    case 35:
                        secEcon();
                        break;
                    case 37:
                        primEcon();
                        break;
                    case 11:
                        CanvObjs["EventCard"].GetComponent<Text>().text = _Event[UnityEngine.Random.Range(0, 11)];
                        CanvObjs["EventCard"].SetActive(true);
                        break;
                    case 33:
                        CanvObjs["EventCard"].GetComponent<Text>().text = _Event[UnityEngine.Random.Range(0, 11)];
                        CanvObjs["EventCard"].SetActive(true);
                        break;


                }
                _opos = Pos;
            }
            #region GameOver
            if (InfectedPeople >= AllPeople)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            #endregion 
            

        }
        public void BuyHealthCenter()
        {
            //check if it belongs to everyone
            if (Bought[Pos] == 0)
            {
                if ( Money > 75)
                {
                    Money -= 75;
                    Bought[Pos] = ownVar.myID;
                }
            }
        }
        public void BuyLab()
        {
            if (Bought[Pos] ==0)
            {
                if (Money > 100)
                {
                    Money -= 100;
                    Bought[Pos] = ownVar.myID;

                }
            }
        }
        public static void FinishedRound()
        {
            ownVar.finishedLevels = 3;
        }
        void HealthCenterField()
        {
           
                if (!CanvObjs["HealthCenter"].activeSelf)
                {
                    CanvObjs["HealthCenter"].SetActive(true);
                }

                if (Bought[Pos] == ownVar.myID)
                {
                    InfectedPeople -= 2;
                }
                else if (Level1[Pos] == ownVar.myID)
                {
                    InfectedPeople -= 5;
                }
                else if (Level2[Pos] == ownVar.myID)
                {
                    InfectedPeople -= 15;
                }
               CanvObjs["PercentageInfected"].GetComponent
                <Text>().text = InfectedPeople.ToString()+"%";
                 

        }
        void researchLabField()
        {
                CanvObjs["ResearchLab"].SetActive(true);
                if (Bought[Pos] == ownVar.myID)
                {
                    Chip++;
                    CanvObjs["Chips"].GetComponent<Text>().text = Chip.ToString();
            }
        }
        void primEcon() {
            CanvObjs["primEcon"].SetActive(true);
        }
        void secEcon() {
            CanvObjs["secEcon"].SetActive(true);
        }
        void terEcon() { 
            CanvObjs["terEcon"].SetActive(true);
        }
        
    }
    }

