using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    #region Variables

    public StateList theStateList;
    public List<Station> activeStations;
    public List<Storage> activeStorages;
    public List<Capsule> activeCapsules;
    public List<Capsule> exitedCapsules;

    public GameManager theGameManager;
    

    #endregion

    private void Awake()
    {
        theGameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        theStateList = new StateList();
        activeStations = new List<Station>();
        activeStorages = new List<Storage>();
        activeCapsules = new List<Capsule>();
        exitedCapsules = new List<Capsule>();

        CreateStationsAndStorages();
    }

    private void Update()
    {
        if (!theGameManager.gamePaused)
        {
            if (activeCapsules != null)
            {
                foreach (Capsule theCapsule in activeCapsules)
                {
                    RunState(theCapsule);
                }
            }
            else
            {
                Debug.Log("activeCapsules is null");
            }

            if (activeStorages != null)
            {
                foreach (Storage theStorage in activeStorages)
                {
                    UpdateStorage(theStorage);
                }
            }
            else
            {
                Debug.Log("activeStorages is null");
            }
        }
        
    }

    #region Classes

    public class StateList
    {
        public List<State> theStateList;

        public StateList()
        {
            theStateList = new List<State>();

            CreateStates();
        }

        #region Methods
        public void CreateStates()
        {
            // add specific objects/methods into each state to specify actions during state

            #region Start

            FSM.State Start1 = new State("Start1", 0, 1);
            Start1.prologMethods.Add("Position,-16,6,8");
            Start1.mainMethods.Add("Movement,1,-16,6,8");
            Start1.epilogMethods.Add("Transition,1,1");
            theStateList.Add(Start1);

            FSM.State Start2 = new State("Start2", 0, 2);
            Start2.prologMethods.Add("Position,-9.6,6,8");
            Start2.mainMethods.Add("Movement,1,-9.6,6,8");
            Start2.epilogMethods.Add("Transition,1,2");
            theStateList.Add(Start2);

            FSM.State Start3 = new State("Start3", 0, 3);
            Start3.prologMethods.Add("Position,-3.2,6,8");
            Start3.mainMethods.Add("Movement,1,-3.2,6,8");
            Start3.epilogMethods.Add("Transition,1,3");
            theStateList.Add(Start3);

            FSM.State Start4 = new State("Start4", 0, 4);
            Start4.prologMethods.Add("Position,3.2,6,8");
            Start4.mainMethods.Add("Movement,1,3.2,6,8");
            Start4.epilogMethods.Add("Transition,1,4");
            theStateList.Add(Start4);

            FSM.State Start5 = new State("Start5", 0, 5);
            Start5.prologMethods.Add("Position,9.6,6,8");
            Start5.mainMethods.Add("Movement,1,9.6,6,8");
            Start5.epilogMethods.Add("Transition,1,5");
            theStateList.Add(Start5);

            FSM.State Start6 = new State("Start6", 0, 6);
            Start6.prologMethods.Add("Position,16,6,8");
            Start6.mainMethods.Add("Movement,1,16,6,8");
            Start6.epilogMethods.Add("Transition,1,6");
            theStateList.Add(Start6);

            #endregion

            #region Start To Intake

            FSM.State Start1toIntake1 = new State("Start1toIntake1", 1, 1);
            Start1toIntake1.mainMethods.Add("Movement,1,-16,6,20");
            Start1toIntake1.epilogMethods.Add("Transition,2,1");
            theStateList.Add(Start1toIntake1);

            FSM.State Start2toIntake2 = new State("Start2toIntake2", 1, 2);
            Start2toIntake2.mainMethods.Add("Movement,1,-9.6,6,20");
            Start2toIntake2.epilogMethods.Add("Transition,2,2");
            theStateList.Add(Start2toIntake2);

            FSM.State Start3toIntake3 = new State("Start3toIntake3", 1, 3);
            Start3toIntake3.mainMethods.Add("Movement,1,-3.2,6,20");
            Start3toIntake3.epilogMethods.Add("Transition,2,3");
            theStateList.Add(Start3toIntake3);

            FSM.State Start4toIntake4 = new State("Start4toIntake4", 1, 4);
            Start4toIntake4.mainMethods.Add("Movement,1,3.2,6,20");
            Start4toIntake4.epilogMethods.Add("Transition,2,4");
            theStateList.Add(Start4toIntake4);

            FSM.State Start5toIntake5 = new State("Start5toIntake5", 1, 5);
            Start5toIntake5.mainMethods.Add("Movement,1,9.6,6,20");
            Start5toIntake5.epilogMethods.Add("Transition,2,5");
            theStateList.Add(Start5toIntake5);

            FSM.State Start6toIntake6 = new State("Start6toIntake6", 1, 6);
            Start6toIntake6.mainMethods.Add("Movement,1,16,6,20");
            Start6toIntake6.epilogMethods.Add("Transition,2,6");
            theStateList.Add(Start6toIntake6);

            #endregion

            #region Intake Station

            FSM.State IntakeStation1 = new State("IntakeStation1", 2, 1);
            IntakeStation1.prologMethods.Add("Position,-16,6,25");
            IntakeStation1.mainMethods.Add("Timer,1");
            IntakeStation1.epilogMethods.Add("Transition,3,1");
            theStateList.Add(IntakeStation1);

            FSM.State IntakeStation2 = new State("IntakeStation2", 2, 2);
            IntakeStation2.prologMethods.Add("Position,-9.6,6,25");
            IntakeStation2.mainMethods.Add("Timer,1");
            IntakeStation2.epilogMethods.Add("Transition,3,2");
            theStateList.Add(IntakeStation2);

            FSM.State IntakeStation3 = new State("IntakeStation3", 2, 3);
            IntakeStation3.prologMethods.Add("Position,-3.2,6,25");
            IntakeStation3.mainMethods.Add("Timer,1");
            IntakeStation3.epilogMethods.Add("Transition,3,3");
            theStateList.Add(IntakeStation3);

            FSM.State IntakeStation4 = new State("IntakeStation4", 2, 4);
            IntakeStation4.prologMethods.Add("Position,3.2,6,25");
            IntakeStation4.mainMethods.Add("Timer,1");
            IntakeStation4.epilogMethods.Add("Transition,3,4");
            theStateList.Add(IntakeStation4);

            FSM.State IntakeStation5 = new State("IntakeStation5", 2, 5);
            IntakeStation5.prologMethods.Add("Position,9.6,6,25");
            IntakeStation5.mainMethods.Add("Timer,1");
            IntakeStation5.epilogMethods.Add("Transition,3,5");
            theStateList.Add(IntakeStation5);

            FSM.State IntakeStation6 = new State("IntakeStation6", 2, 6);
            IntakeStation6.prologMethods.Add("Position,16,6,25");
            IntakeStation6.mainMethods.Add("Timer,1");
            IntakeStation6.epilogMethods.Add("Transition,3,6");
            theStateList.Add(IntakeStation6);

            #endregion

            #region Intake To Weighing

            FSM.State IntakeToWeighing1 = new State("IntakeToWeighing1", 3, 1);
            IntakeToWeighing1.prologMethods.Add("Position,-16,5.85,30");
            IntakeToWeighing1.mainMethods.Add("Movement,1,-18,5.85,48");
            IntakeToWeighing1.epilogMethods.Add("Transition,4,1");
            theStateList.Add(IntakeToWeighing1);

            FSM.State IntakeToWeighing2 = new State("IntakeToWeighing2", 3, 2);
            IntakeToWeighing2.prologMethods.Add("Position,-9.6,5.85,30");
            IntakeToWeighing2.prologMethods.Add("Rotation,0,0,10");
            IntakeToWeighing2.mainMethods.Add("Movement,1,-12.5,5.85,48");
            IntakeToWeighing2.epilogMethods.Add("Transition,4,1");
            theStateList.Add(IntakeToWeighing2);

            FSM.State IntakeToWeighing3 = new State("IntakeToWeighing3", 3, 3);
            IntakeToWeighing3.prologMethods.Add("Position,-3.2,5.85,30");
            IntakeToWeighing3.mainMethods.Add("Movement,1,-3,5.85,48");
            IntakeToWeighing3.epilogMethods.Add("Transition,4,2");
            theStateList.Add(IntakeToWeighing3);

            FSM.State IntakeToWeighing4 = new State("IntakeToWeighing4", 3, 4);
            IntakeToWeighing4.prologMethods.Add("Position,3.2,5.85,30");
            IntakeToWeighing4.mainMethods.Add("Movement,1,3,5.85,48");
            IntakeToWeighing4.epilogMethods.Add("Transition,4,2");
            theStateList.Add(IntakeToWeighing4);

            FSM.State IntakeToWeighing5 = new State("IntakeToWeighing5", 3, 5);
            IntakeToWeighing5.prologMethods.Add("Position,9.6,5.85,30");
            IntakeToWeighing5.prologMethods.Add("Rotation,0,0,-10");
            IntakeToWeighing5.mainMethods.Add("Movement,1,12.5,5.85,48");
            IntakeToWeighing5.epilogMethods.Add("Transition,4,3");
            theStateList.Add(IntakeToWeighing5);

            FSM.State IntakeToWeighing6 = new State("IntakeToWeighing6", 3, 6);
            IntakeToWeighing6.prologMethods.Add("Position,16,5.85,30");
            IntakeToWeighing6.mainMethods.Add("Movement,1,18,5.85,48");
            IntakeToWeighing6.epilogMethods.Add("Transition,4,3");
            theStateList.Add(IntakeToWeighing6);

            #endregion

            #region Weighing Station

            FSM.State WeighingStation1 = new State("WeighingStation1", 4, 1);
            WeighingStation1.prologMethods.Add("Position,-15,7.65,54");
            WeighingStation1.prologMethods.Add("Rotation,0,0,0");
            WeighingStation1.mainMethods.Add("Timer,2");
            WeighingStation1.epilogMethods.Add("Transition,5,1");
            theStateList.Add(WeighingStation1);

            FSM.State WeighingStation2 = new State("WeighingStation2", 4, 2);
            WeighingStation2.prologMethods.Add("Position,0,7.65,54");
            WeighingStation2.prologMethods.Add("Rotation,0,0,0");
            WeighingStation2.mainMethods.Add("Timer,2");
            WeighingStation2.epilogMethods.Add("Transition,5,2");
            theStateList.Add(WeighingStation2);

            FSM.State WeighingStation3 = new State("WeighingStation3", 4, 3);
            WeighingStation3.prologMethods.Add("Position,15,7.65,54");
            WeighingStation3.prologMethods.Add("Rotation,0,0,0");
            WeighingStation3.mainMethods.Add("Timer,5");
            WeighingStation3.epilogMethods.Add("Transition,5,3");
            theStateList.Add(WeighingStation3);

            #endregion

            #region Weighing To Packaging

            FSM.State WeighingToPackaging1 = new State("WeighingToPackaging1", 5, 1);
            WeighingToPackaging1.prologMethods.Add("Position,-15,5.85,59.5");
            WeighingToPackaging1.mainMethods.Add("Movement,1,-15,5.85,84");
            WeighingToPackaging1.epilogMethods.Add("Transition,6,1");
            theStateList.Add(WeighingToPackaging1);

            FSM.State WeighingToPackaging2 = new State("WeighingToPackaging2", 5, 2);
            WeighingToPackaging2.prologMethods.Add("Position,0,5.85,59.5");
            WeighingToPackaging2.mainMethods.Add("Movement,1,0,5.85,84");
            WeighingToPackaging2.epilogMethods.Add("Transition,6,1");
            theStateList.Add(WeighingToPackaging2);

            FSM.State WeighingToPackaging3 = new State("WeighingToPackaging3", 5, 3);
            WeighingToPackaging3.prologMethods.Add("Position,15,5.85,59.5");
            WeighingToPackaging3.mainMethods.Add("Movement,1,15,5.85,84");
            WeighingToPackaging3.epilogMethods.Add("Transition,6,1");
            theStateList.Add(WeighingToPackaging3);

            #endregion

            FSM.State PackagingStation = new State("PackagingStation", 6, 1);
            PackagingStation.prologMethods.Add("Position,0,5.85,90");
            PackagingStation.prologMethods.Add("Package");
            PackagingStation.mainMethods.Add("Timer,2");
            PackagingStation.epilogMethods.Add("Transition,7,1");
            theStateList.Add(PackagingStation);

            FSM.State PackagingToInspection = new State("PackagingToInspection", 7, 1);
            PackagingToInspection.prologMethods.Add("Position,0,5.85,112");
            PackagingToInspection.mainMethods.Add("Movement,1,0,5.85,134");
            PackagingToInspection.epilogMethods.Add("Transition,8,1");
            theStateList.Add(PackagingToInspection);

            FSM.State InspectionStation = new State("InspectionStation", 8, 1);
            InspectionStation.prologMethods.Add("Position,0,5.85,134");
            InspectionStation.mainMethods.Add("Movement,2,0,5.85,158");
            InspectionStation.epilogMethods.Add("Transition,9,1");
            theStateList.Add(InspectionStation);
             
            FSM.State InspectionToProcessing = new State("InspectionToProcessing", 9, 1);
            InspectionToProcessing.prologMethods.Add("Position,0,4.2,160");
            InspectionToProcessing.prologMethods.Add("Rotation, -30, 0, 0");
            InspectionToProcessing.mainMethods.Add("Movement,1,0,15.5,180");
            InspectionToProcessing.epilogMethods.Add("Transition,10,1");
            theStateList.Add(InspectionToProcessing);
            
            FSM.State ProcessingStation = new State("ProcessingStation", 10, 1);
            ProcessingStation.prologMethods.Add("Position,16,16,185");
            ProcessingStation.mainMethods.Add("Timer,3");
            ProcessingStation.epilogMethods.Add("Transition,11,1");
            theStateList.Add(ProcessingStation);

            FSM.State ProcessingToDrop = new State("ProcessingToDrop", 11, 1);
            ProcessingToDrop.prologMethods.Add("Position,40,16.5,186");
            ProcessingToDrop.prologMethods.Add("Rotation, 0, 0, 90");
            ProcessingToDrop.mainMethods.Add("Split,1,65,16.5,186,75,16.5,186,85,16.5,186");
            ProcessingToDrop.epilogMethods.Add("Transition,12,1");
            theStateList.Add(ProcessingToDrop);

            FSM.State DropDown = new State("DropDown", 12, 1);
            DropDown.mainMethods.Add("Split,4,65,2.5,186,75,2.5,186,85,2.5,186");
            DropDown.epilogMethods.Add("Transition,13,1");
            theStateList.Add(DropDown);

            FSM.State HorizontalBelt = new State("HorizontalBelt", 13, 1);
            HorizontalBelt.mainMethods.Add("Split,2,65,2.5,156.75,75,2.5,156.75,85,2.5,156.75");
            HorizontalBelt.epilogMethods.Add("Transition,14,1");
            theStateList.Add(HorizontalBelt);

            FSM.State UpwardBelt = new State("UpwardBelt", 14, 1);
            UpwardBelt.prologMethods.Add("Rotation, 30, 0, 0");
            UpwardBelt.mainMethods.Add("Split,1,65,12.5,139.5,75,12.5,139.5,85,12.5,139.5");
            UpwardBelt.epilogMethods.Add("Store");
            theStateList.Add(UpwardBelt);
            
            FSM.State SolidStorage = new State("SolidStorage", 15, 1);
            theStateList.Add(SolidStorage);

            FSM.State LiquidStorage = new State("LiquidStorage", 15, 2);
            theStateList.Add(LiquidStorage);

            FSM.State GasStorage = new State("GasStorage", 15, 3);
            theStateList.Add(GasStorage);

            FSM.State StorageToExit1 = new State("StorageToExit1", 16, 1);
            StorageToExit1.mainMethods.Add("Movement,2,65,12.5,55");
            StorageToExit1.epilogMethods.Add("Transition,17,1");
            theStateList.Add(StorageToExit1);

            FSM.State StorageToExit2 = new State("StorageToExit2", 16, 2);
            StorageToExit2.mainMethods.Add("Movement,2,75,12.5,55");
            StorageToExit2.epilogMethods.Add("Transition,17,2");
            theStateList.Add(StorageToExit2);

            FSM.State StorageToExit3 = new State("StorageToExit3", 16, 3);
            StorageToExit3.mainMethods.Add("Movement,2,85,12.5,55");
            StorageToExit3.epilogMethods.Add("Transition,17,3");
            theStateList.Add(StorageToExit3);

            FSM.State ExitStation1 = new State("ExitStation1", 17, 1);
            ExitStation1.mainMethods.Add("Timer,0.2");
            ExitStation1.epilogMethods.Add("Transition,18,1");
            theStateList.Add(ExitStation1);

            FSM.State ExitStation2 = new State("ExitStation2", 17, 2);
            ExitStation2.mainMethods.Add("Timer,0.2");
            ExitStation2.epilogMethods.Add("Transition,18,2");
            theStateList.Add(ExitStation2);

            FSM.State ExitStation3 = new State("ExitStation3", 17, 3);
            ExitStation3.mainMethods.Add("Timer,0.2");
            ExitStation3.epilogMethods.Add("Transition,18,3");
            theStateList.Add(ExitStation3);

            FSM.State Exit1 = new State("Exit1", 18, 1);
            Exit1.mainMethods.Add("Movement,2,65,12.5,32");
            Exit1.epilogMethods.Add("Exit");
            theStateList.Add(Exit1);

            FSM.State Exit2 = new State("Exit2", 18, 2);
            Exit2.mainMethods.Add("Movement,2,75,12.5,32");
            Exit2.epilogMethods.Add("Exit");
            theStateList.Add(Exit2);

            FSM.State Exit3 = new State("Exit3", 18, 3);
            Exit3.mainMethods.Add("Movement,2,85,12.5,32");
            Exit3.epilogMethods.Add("Exit");
            theStateList.Add(Exit3);

        }

        #endregion
    }

    public class State
    {
        public string name;
        public int sequenceNumber;
        public int stateNumber;
        public List<string> prologMethods;
        public List<string> mainMethods;
        public List<string> epilogMethods;

        public State(string theStateName, int theSequenceNumber, int theStateNumber)
        {
            name = theStateName;
            sequenceNumber = theSequenceNumber;
            stateNumber = theStateNumber;

            prologMethods = new List<string>();
            mainMethods = new List<string>();
            epilogMethods = new List<string>();
        }
    }

    #endregion

    #region Methods

    #region Run State Methods
    public void RunState(Capsule theCapsule)
    {
        State currentState = theCapsule.currentState;
        Station currentStation = new Station("temp", currentState, 0f);
        Storage currentStorage = new Storage("temp", currentState);
        bool isStation = false;
        bool isStorage = false;

        foreach (Station theStation in activeStations)
        {
            if (theStation.name.Equals(currentState.name))
            {
                currentStation = theStation;
                isStation = true;
            }
        }

        foreach (Storage theStorage in activeStorages)
        {
            if (theStorage.name.Equals(currentState.name))
            {
                currentStorage = theStorage;
                isStorage = true;
            }
        }

        if (theCapsule.inProlog)
        {
            if (isStorage)
            {
                InsertStorage(theCapsule, currentStorage);
            }
            else
            {
                RunStateProlog(theCapsule); // one-time run at start of state

                if (isStation)
                {
                    currentStation.queue.Enqueue(theCapsule);
                    theCapsule.inQueue = true;
                }
            }            
        }
        else if (theCapsule.inMain)
        {
            if (isStation)
            {
                if (currentStation.queue.Count > 0 && currentStation.queue.Peek() == theCapsule)
                {
                    RunStateMain(theCapsule); // continuous run during state
                }
            } else
            {
                RunStateMain(theCapsule); // continuous run during state
            }
        }
        else if (theCapsule.inEpilog)
        {
            if (isStation)
            {
                currentStation.queue.Dequeue();
            }
            RunStateEpilog(theCapsule); // one-time run at end of state
        }
        else
        {
            Debug.Log("Capsule not in prolog, main or epilog");
        }


    }
    
    public void RunStateProlog(Capsule theCapsule)
    {
        theCapsule.inProlog = false;
        theCapsule.inMain = true;
        if (theCapsule.currentState.Equals("Exit1")) Debug.Log("reached prolog");
        foreach (string method in theCapsule.currentState.prologMethods)
        {
            string[] input = method.Split(',');

            ExecuteMethod(theCapsule, input);
        }
    }

    public void RunStateMain(Capsule theCapsule)
    {
        if (theCapsule.currentState.Equals("Exit1")) Debug.Log("reached main");

        foreach (string method in theCapsule.currentState.mainMethods)
        {
            string[] input = method.Split(',');

            ExecuteMethod(theCapsule, input);
        }
    }

    public void RunStateEpilog(Capsule theCapsule)
    {
        theCapsule.inEpilog = false;
        theCapsule.inProlog = true;

        foreach (string method in theCapsule.currentState.epilogMethods)
        {
            string[] input = method.Split(',');

            ExecuteMethod(theCapsule, input);
        }
    }

    #endregion

    #region Perform Methods

    public void ExecuteMethod(Capsule theCapsule, string[] input)
    {
        switch (input[0])
        {
            case "Position":
                PerformPosition(theCapsule, input);
                break;
            case "Movement":
                PerformMovement(theCapsule, input);
                break;
            case "Timer":
                PerformTimer(theCapsule, input);
                break;
            case "Transition":
                PerformTransition(theCapsule, input);
                break;
            case "Rotation":
                PerformRotation(theCapsule, input);
                break;
            case "Split":
                PerformSplit(theCapsule, input);
                break;
            case "Package":
                PerformPackage(theCapsule, input);
                break;
            case "Store":
                PerformStore(theCapsule, input);
                break;
            case "Exit":
                PerformExit(theCapsule, input);
                break;
            default:
                Debug.Log("method not found in RunState for: " + input[0]);
                break;
        }
    }

    public void PerformPosition(Capsule theCapsule, string[] input)
    {
        float xPos = float.Parse(input[1]);
        float yPos = float.Parse(input[2]);
        float zPos = float.Parse(input[3]);

        theCapsule.capsuleObject.transform.position = new Vector3(xPos, yPos, zPos);
    }

    public void PerformMovement(Capsule theCapsule, string[] input)
    {
        float speed = float.Parse(input[1]);
        float xPos = float.Parse(input[2]);
        float yPos = float.Parse(input[3]);
        float zPos = float.Parse(input[4]);

        Vector3 currentPosition = theCapsule.capsuleObject.transform.position;
        Vector3 targetPosition = new Vector3(xPos, yPos, zPos);
        
        if (Vector3.Distance(currentPosition, targetPosition) <= 0.1f)
        {
            theCapsule.capsuleObject.transform.position = targetPosition;
            theCapsule.inMain = false;
            theCapsule.inEpilog = true;
        }
        else
        {
            theCapsule.capsuleObject.transform.SetPositionAndRotation(Vector3.MoveTowards(currentPosition, targetPosition,
                2 * speed * Time.deltaTime), theCapsule.capsuleObject.transform.rotation);
        }
    }

    public void PerformTimer(Capsule theCapsule, string[] input)
    {
        float timerTarget = float.Parse(input[1]);

        if (theCapsule.timer < timerTarget)
        {
            theCapsule.timer += Time.deltaTime;
        } else
        {
            theCapsule.timer = 0;
            theCapsule.inMain = false;
            theCapsule.inEpilog = true;
        }
    }

    public void PerformTransition(Capsule theCapsule, string[] input)
    {
        int sequenceNumber = int.Parse(input[1]);
        int stateNumber = int.Parse(input[2]);

        if (sequenceNumber > 0 && stateNumber > 0)
        {
            theCapsule.currentState = FindState(sequenceNumber, stateNumber);
        }
    }

    public void PerformRotation(Capsule theCapsule, string[] input)
    {
        float xRot = float.Parse(input[1]) + 90;
        float yRot = float.Parse(input[2]);
        float zRot = float.Parse(input[3]);

        Vector3 rotationVector = new Vector3(xRot, yRot, zRot);

        theCapsule.capsuleObject.transform.rotation = Quaternion.Euler(rotationVector);
    }

    public void PerformSplit(Capsule theCapsule, string[] input)
    {
        float speed = float.Parse(input[1]);

        float solidX = float.Parse(input[2]);
        float solidY = float.Parse(input[3]);
        float solidZ = float.Parse(input[4]);

        float liquidX = float.Parse(input[5]);
        float liquidY = float.Parse(input[6]);
        float liquidZ = float.Parse(input[7]);

        float gasX = float.Parse(input[8]);
        float gasY = float.Parse(input[9]);
        float gasZ = float.Parse(input[10]);

        if (theCapsule.stateOfMatter.Equals("solid"))
        {
            SplitHelper(theCapsule, speed, solidX, solidY, solidZ);
        }
        else if (theCapsule.stateOfMatter.Equals("liquid"))
        {
            SplitHelper(theCapsule, speed, liquidX, liquidY, liquidZ);
        }
        else if (theCapsule.stateOfMatter.Equals("gas"))
        {
            SplitHelper(theCapsule, speed, gasX, gasY, gasZ);
        }
        else
        {
            Debug.Log(theCapsule.stateOfMatter + " State of matter not found in PerformSplit");
        }

    }


    public void SplitHelper(Capsule theCapsule, float speed, float xPos, float yPos, float zPos)
    {
        Vector3 currentPosition = theCapsule.capsuleObject.transform.position;
        Vector3 targetPosition = new Vector3(xPos, yPos, zPos);

        if (Vector3.Distance(currentPosition, targetPosition) <= 0.1f)
        {
            theCapsule.capsuleObject.transform.position = targetPosition;
            theCapsule.inMain = false;
            theCapsule.inEpilog = true;
        }
        else
        {
            theCapsule.capsuleObject.transform.SetPositionAndRotation(Vector3.MoveTowards(currentPosition, targetPosition,
                2 * speed * Time.deltaTime), theCapsule.capsuleObject.transform.rotation);
        }
    }
    public void PerformPackage(Capsule theCapsule, string[] input)
    {
        GameObject oldObject = theCapsule.capsuleObject;

        GameObject parent = Instantiate(theGameManager.goldenBox);
        theCapsule.capsuleObject = Instantiate(theGameManager.goldenBox, parent.transform);
        theCapsule.capsuleObject.GetComponent<MeshRenderer>().material = theCapsule.material;
        theCapsule.capsuleObject.transform.SetPositionAndRotation(oldObject.transform.position, oldObject.transform.rotation);

        Destroy(oldObject);
    }

    public void PerformStore(Capsule theCapsule, string[] input)
    {
        theCapsule.capsuleObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        if (theCapsule.stateOfMatter.Equals("solid"))
        {
            theCapsule.currentState = FindState(15, 1);
        }
        else if (theCapsule.stateOfMatter.Equals("liquid"))
        {
            theCapsule.currentState = FindState(15, 2);
        }
        else if (theCapsule.stateOfMatter.Equals("gas"))
        {
            theCapsule.currentState = FindState(15, 3);
        }
    }

    public void PerformExit(Capsule theCapsule, string[] input)
    {
        if (!exitedCapsules.Contains(theCapsule))
        {
            exitedCapsules.Add(theCapsule);
            theCapsule.capsuleObject.SetActive(false);
        }

    }

    #endregion

    #region Storage Methods

    public void InsertStorage(Capsule theCapsule, Storage theStorage)
    {
        Vector3 currentPosition = theCapsule.capsuleObject.transform.position;
        Vector3 targetPosition = new Vector3(currentPosition.x, currentPosition.y, 105f + 3f * theStorage.queue.Count);

        if (Vector3.Distance(currentPosition, targetPosition) <= 0.1f)
        {
            theCapsule.capsuleObject.transform.position = targetPosition;
            theCapsule.inMain = true;
            theCapsule.inProlog = false;

            theStorage.queue.Enqueue(theCapsule);
            theCapsule.inQueue = true;
        }
        else
        {
            theCapsule.capsuleObject.transform.SetPositionAndRotation(Vector3.MoveTowards(currentPosition, targetPosition,
                10 * Time.deltaTime), theCapsule.capsuleObject.transform.rotation);
        }
    }

    public void UpdateStorage( Storage theStorage)
    {
        if (theStorage.queue.Count >= theGameManager.storageCapacity)
        {
            while (theStorage.queue.Count > 0)
            {
                Capsule theCapsule = theStorage.queue.Dequeue();
                theCapsule.inQueue = false;
                theCapsule.inProlog = true;
                theCapsule.inMain = false;

                if (theCapsule.stateOfMatter.Equals("solid"))
                {
                    theCapsule.currentState = FindState(16, 1);
                }
                else if (theCapsule.stateOfMatter.Equals("liquid"))
                {
                    theCapsule.currentState = FindState(16, 2);
                }
                else if (theCapsule.stateOfMatter.Equals("gas"))
                {
                    theCapsule.currentState = FindState(16, 3);
                }

            }
        }
    }

    #endregion

    public void CreateStationsAndStorages()
    {
        Station IntakeStation1 = new Station("IntakeStation1", FindState(2, 1), 1f);
        activeStations.Add(IntakeStation1);
        Station IntakeStation2 = new Station("IntakeStation2", FindState(2, 2), 1f);
        activeStations.Add(IntakeStation2);
        Station IntakeStation3 = new Station("IntakeStation3", FindState(2, 3), 1f);
        activeStations.Add(IntakeStation3);
        Station IntakeStation4 = new Station("IntakeStation4", FindState(2, 4), 1f);
        activeStations.Add(IntakeStation4);
        Station IntakeStation5 = new Station("IntakeStation5", FindState(2, 5), 1f);
        activeStations.Add(IntakeStation5);
        Station IntakeStation6 = new Station("IntakeStation6", FindState(2, 6), 1f);
        activeStations.Add(IntakeStation6);

        Station WeighingStation1 = new Station("WeighingStation1", FindState(4, 1), 2f);
        activeStations.Add(WeighingStation1);
        Station WeighingStation2 = new Station("WeighingStation2", FindState(4, 2), 2f);
        activeStations.Add(WeighingStation2);
        Station WeighingStation3 = new Station("WeighingStation3", FindState(4, 3), 5f);
        activeStations.Add(WeighingStation3);

        Station PackagingStation = new Station("PackagingStation", FindState(6, 1), 2f);
        activeStations.Add(PackagingStation);

        Station InspectionStation = new Station("InspectionStation", FindState(8, 1), 6f);
        activeStations.Add(InspectionStation);

        Station ProcessingStation = new Station("ProcessingStation", FindState(10, 1), 3f);
        activeStations.Add(ProcessingStation);

        Storage solidStorage = new Storage("SolidStorage", FindState(15, 1));
        activeStorages.Add(solidStorage);
        Storage liquidStorage = new Storage("LiquidStorage", FindState(15, 2));
        activeStorages.Add(liquidStorage);
        Storage gasStorage = new Storage("GasStorage", FindState(15, 3));
        activeStorages.Add(gasStorage);
    }

    public State FindState(int theSequenceNumber, int theStateNumber)
    {
        State foundState = theStateList.theStateList[0];

        foreach (State theState in theStateList.theStateList)
        {
            if (theState.sequenceNumber == theSequenceNumber && theState.stateNumber == theStateNumber)
            {
                foundState = theState;
            }
        }

        return foundState;
    }

    public Station FindStation(string stationName)
    {
        Station resultStation = new Station("notFound", new State("null", 0, 0), 0);

        foreach (Station theStation in activeStations)
        {
            if (theStation.name.ToLower().Equals(stationName.ToLower()))
            {
                resultStation = theStation;
                break;
            }
        }

        return resultStation;
    }

    public Storage FindStorage(string storageName)
    {
        Storage resultStorage = new Storage("notFound", new State("null", 0, 0));

        foreach (Storage theStorage in activeStorages)
        {
            if (theStorage.name.Equals(storageName))
            {
                resultStorage = theStorage;
                break;
            }
        }

        return resultStorage;
    }
    #endregion
}
