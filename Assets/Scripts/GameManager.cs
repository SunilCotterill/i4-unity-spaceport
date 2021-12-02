using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    // Game Objects
    public FSM theFSM;
    public GameObject goldenCapsule;
    public GameObject goldenBox;
    public List<GameObject> stationInfoObjects;
    public GameObject gameMenuCanvas;
    public GameObject gameCanvas;
    public GameObject worldCanvases;
    public GameObject buttonTemplate;
    public GameObject stationInfoPanelTemplate;
    public GameObject storageInfoPanelTemplate;
    public GameObject capsuleInfoPanelTemplate;
    public AudioMixer audioMixer;

    // Materials
    public Material gasCapsuleMaterial;
    public Material liquidCapsuleMaterial;
    public Material solidCapsuleMaterial;
    
    // temp vars
    private float canSpawnIntake1 = 0f;
    private float canSpawnIntake2 = 0f;
    private float canSpawnIntake3 = 0f;
    private float canSpawnIntake4 = 0f;
    private float canSpawnIntake5 = 0f;
    private float canSpawnIntake6 = 0f;
    public bool stationButtonsSet = false;
    public List<GameObject> infoPanels;
    public Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    // Game Variables
    [SerializeField]
    public bool gamePaused = false;
    [SerializeField]
    public bool continueSim = false;
    [SerializeField]
    public float spawnFrequency = 1f;
    [SerializeField]
    public float storageCapacity = 5f;


    // Start is called before the first frame update
    void Awake()
    {
        theFSM = GameObject.FindGameObjectWithTag("FSM").GetComponent<FSM>();
        gameMenuCanvas = GameObject.FindGameObjectWithTag("GameMenuCanvas");
        gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas");

        CreateResolutions();

        gameCanvas.SetActive(true);
        gameMenuCanvas.SetActive(false);

        infoPanels = new List<GameObject>();

        if (theFSM.activeCapsules == null)
        {
            theFSM.activeCapsules = new List<Capsule>();
            theFSM.exitedCapsules = new List<Capsule>();
        }
        
        if (theFSM.theStateList == null)
        {
            theFSM.theStateList = new FSM.StateList();
        }

        continueSim = true;
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused)
            {
                LeaveGameMenus();
            }
            else
            {
                EnterGameMenus();
            }
        }

        // In-Game Commands
        if (!gamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Time.timeScale *= 0.5f;
                Debug.Log("Time multiplier: " + Time.timeScale);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                Time.timeScale *= 2f;
                Debug.Log("Time multiplier: " + Time.timeScale);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                Time.timeScale = 1f;
                Debug.Log("Time multiplier: " + Time.timeScale);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                BeginSimulation();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                continueSim = !continueSim;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SpawnCapsule(theFSM.FindState(0, 1), "liquid");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SpawnCapsule(theFSM.FindState(0, 2), "gas");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SpawnCapsule(theFSM.FindState(0, 3), "solid");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SpawnCapsule(theFSM.FindState(0, 4), "solid");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SpawnCapsule(theFSM.FindState(0, 5), "gas");
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SpawnCapsule(theFSM.FindState(0, 6), "solid");
            }

            RunSimulation();

            UpdateSpawnTimers();

            UpdateWorldCanvasInfo();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Transform hitObjectTransform = hit.transform;

                Transform tagContainer = gameCanvas.transform.GetChild(1);

                if (tagContainer.gameObject.activeSelf == false)
                {
                    tagContainer.gameObject.SetActive(true);
                }

                if (hitObjectTransform.name.Equals("Ground") || 
                    hitObjectTransform.name.Equals("Capsule(Clone)") || 
                    hitObjectTransform.name.Equals("Platform") ||
                    hitObjectTransform.name.Equals("Rails") ||
                    hitObjectTransform.name.Equals("Ramp") ||
                    hitObjectTransform.name.Equals("Stands") ||
                    hitObjectTransform.name.StartsWith("Cylinder") ||
                    hitObjectTransform.name.StartsWith("Cube"))
                {
                    tagContainer.gameObject.SetActive(false);
                } else
                {
                    Text theText = tagContainer.GetChild(0).GetComponent<Text>();
                    theText.text = hitObjectTransform.name;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    EnterGameMenus();

                    if (hitObjectTransform.name.ToLower().Contains("station"))
                    {
                        OpenStationInfoMenu(hitObjectTransform.name);
                    }
                    if (hitObjectTransform.name.ToLower().Contains("storage"))
                    {
                        OpenStorageInfoMenu(hitObjectTransform.name);
                    }
                }
            }

        }
    }

    public void BeginSimulation()
    {
        GameObject parent = Instantiate(goldenCapsule);
        //parent.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        string solid = "solid";
        string liquid = "liquid";
        string gas = "gas";

        Capsule intake1 = new Capsule(Instantiate(goldenCapsule, parent.transform), theFSM.FindState(0, 1), liquid, liquidCapsuleMaterial);
        Capsule intake2 = new Capsule(Instantiate(goldenCapsule, parent.transform), theFSM.FindState(0, 2), gas, gasCapsuleMaterial);
        Capsule intake3 = new Capsule(Instantiate(goldenCapsule, parent.transform), theFSM.FindState(0, 3), solid, solidCapsuleMaterial);
        Capsule intake4 = new Capsule(Instantiate(goldenCapsule, parent.transform), theFSM.FindState(0, 4), solid, gasCapsuleMaterial);
        Capsule intake5 = new Capsule(Instantiate(goldenCapsule, parent.transform), theFSM.FindState(0, 5), gas, solidCapsuleMaterial);
        Capsule intake6 = new Capsule(Instantiate(goldenCapsule, parent.transform), theFSM.FindState(0, 6), solid, solidCapsuleMaterial);

        theFSM.activeCapsules.Add(intake1);
        theFSM.activeCapsules.Add(intake2);
        theFSM.activeCapsules.Add(intake3);
        theFSM.activeCapsules.Add(intake4);
        theFSM.activeCapsules.Add(intake5);
        theFSM.activeCapsules.Add(intake6);

        foreach (Capsule theCapsule in theFSM.activeCapsules)
        {
            theCapsule.capsuleObject.transform.SetPositionAndRotation(theCapsule.capsuleObject.transform.position, Quaternion.Euler(new Vector3(90f, 0, 0)));
        }
    }

    public void RunSimulation()
    {
        float spawnBlocker = 1f;

        if (continueSim)
        {
            if (canSpawnIntake1 <= 0f)
            {
                bool spawned = RandomSpawn(theFSM.FindState(0, 1), "liquid");
                if (spawned)
                {
                    canSpawnIntake1 = spawnBlocker;
                }
            }
            if (canSpawnIntake2 <= 0f)
            {
                bool spawned = RandomSpawn(theFSM.FindState(0, 2), "gas");
                if (spawned)
                {
                    canSpawnIntake2 = spawnBlocker;
                }
            }
            if (canSpawnIntake3 <= 0f)
            {
                bool spawned = RandomSpawn(theFSM.FindState(0, 3), "solid");
                if (spawned)
                {
                    canSpawnIntake3 = spawnBlocker;
                }
            }
            if (canSpawnIntake4 <= 0f)
            {
                bool spawned = RandomSpawn(theFSM.FindState(0, 4), "solid");
                if (spawned)
                {
                    canSpawnIntake4 = spawnBlocker;
                }
            }
            if (canSpawnIntake5 <= 0f)
            {
                bool spawned = RandomSpawn(theFSM.FindState(0, 5), "gas");
                if (spawned)
                {
                    canSpawnIntake5 = spawnBlocker;
                }
            }
            if (canSpawnIntake6 <= 0f)
            {
                bool spawned = RandomSpawn(theFSM.FindState(0, 6), "solid");
                if (spawned)
                {
                    canSpawnIntake6 = spawnBlocker;
                }
            }
        }
    }

    public void UpdateSpawnTimers()
    {
        if (canSpawnIntake1 > 0f)
        {
            canSpawnIntake1 -= Time.deltaTime;
        }
        if (canSpawnIntake2 > 0f)
        {
            canSpawnIntake2 -= Time.deltaTime;
        }
        if (canSpawnIntake3 > 0f)
        {
            canSpawnIntake3 -= Time.deltaTime;
        }
        if (canSpawnIntake4 > 0f)
        {
            canSpawnIntake4 -= Time.deltaTime;
        }
        if (canSpawnIntake5 > 0f)
        {
            canSpawnIntake5 -= Time.deltaTime;
        }
        if (canSpawnIntake6 > 0f)
        {
            canSpawnIntake6 -= Time.deltaTime;
        }
    }

    public bool RandomSpawn(FSM.State theState, string theStateOfMatter)
    {
        bool spawned = false;
        float convertedSpawnFrequency = spawnFrequency * 0.001f;

        // 1 = 0.1% chance of spawning

        float randomNumber = Random.Range(0.000f, 1.000f);

        if (randomNumber < convertedSpawnFrequency)
        {
            SpawnCapsule(theState, theStateOfMatter);
            spawned = true;
        }
        return spawned;
    }

    public void SpawnCapsule(FSM.State state, string stateOfMatter)
    {

        GameObject parent = Instantiate(goldenCapsule);
        Material theMaterial = null;

        switch (stateOfMatter)
        {
            case "solid":
                theMaterial = solidCapsuleMaterial;
                break;
            case "liquid":
                theMaterial = liquidCapsuleMaterial;
                break;
            case "gas":
                theMaterial = gasCapsuleMaterial;
                break;
            default:
                Debug.Log("Material not found for " + stateOfMatter + " (SpawnCapsule)");
                break;
        }

        Capsule theCapsule = new Capsule(Instantiate(goldenCapsule, parent.transform), state, stateOfMatter, theMaterial);
        theCapsule.capsuleObject.transform.SetPositionAndRotation(theCapsule.capsuleObject.transform.position, Quaternion.Euler(new Vector3(90f, 0, 0)));
        theFSM.activeCapsules.Add(theCapsule);
    }

    public void SetSpawnFrequency(float spawnRate)
    {
        spawnFrequency = spawnRate;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LeaveGameMenus()
    {
        CloseGameMenus();

        gamePaused = false;
        gameCanvas.SetActive(true);
        gameMenuCanvas.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnterGameMenus()
    {
        gamePaused = true;
        gameCanvas.SetActive(false);
        gameMenuCanvas.SetActive(true);
        OpenPauseMenu();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void CloseGameMenus()
    {
        List<GameObject> menus = new List<GameObject>();
        menus.Add(gameMenuCanvas.transform.GetChild(0).gameObject); // pause menu
        menus.Add(gameMenuCanvas.transform.GetChild(1).gameObject); // data menu
        menus.Add(gameMenuCanvas.transform.GetChild(2).gameObject); // stations menu
        menus.Add(gameMenuCanvas.transform.GetChild(3).gameObject); // states menu
        menus.Add(gameMenuCanvas.transform.GetChild(4).gameObject); // station info menu
        menus.Add(gameMenuCanvas.transform.GetChild(5).gameObject); // storage info menu
        menus.Add(gameMenuCanvas.transform.GetChild(6).gameObject); // capsule info menu
        menus.Add(gameMenuCanvas.transform.GetChild(7).gameObject); // settings menu
        menus.Add(gameMenuCanvas.transform.GetChild(8).gameObject); // controls menu
        menus.Add(gameMenuCanvas.transform.GetChild(9).gameObject); // preferences menu
        menus.Add(gameMenuCanvas.transform.GetChild(10).gameObject); // credits menu

        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
    }

    public void OpenPauseMenu()
    {
        CloseGameMenus();

        GameObject pauseMenu = gameMenuCanvas.transform.GetChild(0).gameObject;
        pauseMenu.SetActive(true);
    }

    public void OpenDataMenu()
    {
        CloseGameMenus();

        GameObject dataMenu = gameMenuCanvas.transform.GetChild(1).gameObject;
        dataMenu.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        CloseGameMenus();

        GameObject settingsMenu = gameMenuCanvas.transform.GetChild(7).gameObject;
        settingsMenu.SetActive(true);
    }

    public void OpenControlsMenu()
    {
        CloseGameMenus();

        GameObject controlsMenu = gameMenuCanvas.transform.GetChild(8).gameObject;
        controlsMenu.SetActive(true);
    }

    public void OpenPreferencesMenu()
    {
        CloseGameMenus();

        GameObject preferencesMenu = gameMenuCanvas.transform.GetChild(9).gameObject;
        preferencesMenu.SetActive(true);
    }

    public void OpenCreditsMenu()
    {
        CloseGameMenus();

        GameObject creditsMenu = gameMenuCanvas.transform.GetChild(10).gameObject;
        creditsMenu.SetActive(true);
    }

    public void OpenStationsMenu()
    {
        CloseGameMenus();

        GameObject stationsMenu = gameMenuCanvas.transform.GetChild(2).gameObject;
        stationsMenu.SetActive(true);

        if (! stationButtonsSet)
        {
            GenerateStationsButtons();
            stationButtonsSet = true;
        }
    }


    public void OpenStationInfoMenu(string stationName)
    {
        CloseGameMenus();

        GameObject stationInfoMenu = gameMenuCanvas.transform.GetChild(4).gameObject;
        stationInfoMenu.SetActive(true);

        foreach (GameObject theObject in stationInfoObjects)
        {
            Destroy(theObject);
        }

        if (stationName.ToLower().Contains(("intake")))
        {
            #region Intake Station 1

            GameObject intakeStation1Object = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(intakeStation1Object);
            intakeStation1Object.SetActive(true);
            intakeStation1Object.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station intakeStation1 = theFSM.FindStation("IntakeStation1");

            intakeStation1Object.transform.GetChild(0).GetComponent<Text>().text = "Intake Station 1";
            intakeStation1Object.transform.GetChild(3).GetComponent<Text>().text = 
                "This station is the first step in processing the liquids that are sent to the station. " +
                "It ensures the liquid is something that the station is capable of handling and sends it to the next station.";
            intakeStation1Object.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + intakeStation1.speed + " seconds";
            if (intakeStation1.queue.Count > 0)
            {
                intakeStation1Object.transform.GetChild(6).GetComponent<Text>().text = intakeStation1.queue.Peek().capsuleGUID;
            }
            else
            {
                intakeStation1Object.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            intakeStation1Object.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + intakeStation1.queue.Count + "): ";

            if (intakeStation1.queue.Count > 1)
            {
                foreach (Capsule theCapsule in intakeStation1.queue)
                {
                    intakeStation1Object.transform.GetChild(8).GetComponent<Text>().text =
                        intakeStation1Object.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                intakeStation1Object.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }

            #endregion

            #region Intake Station 2

            GameObject intakeStation2Object = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(intakeStation2Object);
            intakeStation2Object.SetActive(true);
            intakeStation2Object.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station intakeStation2 = theFSM.FindStation("IntakeStation2");

            intakeStation2Object.transform.GetChild(0).GetComponent<Text>().text = "Intake Station 2";
            intakeStation2Object.transform.GetChild(3).GetComponent<Text>().text =
                "This station is the first step in processing the gasses that are sent to the station. " +
                "It ensures the gas is something that the station is capable of handling and sends it to the next station.";
            intakeStation2Object.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + intakeStation2.speed + " seconds";
            if (intakeStation2.queue.Count > 0)
            {
                intakeStation2Object.transform.GetChild(6).GetComponent<Text>().text = intakeStation2.queue.Peek().capsuleGUID;
            }
            else
            {
                intakeStation2Object.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            intakeStation2Object.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + intakeStation2.queue.Count + "): ";

            if (intakeStation2.queue.Count > 1)
            {
                foreach (Capsule theCapsule in intakeStation2.queue)
                {
                    intakeStation2Object.transform.GetChild(8).GetComponent<Text>().text =
                        intakeStation2Object.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                intakeStation2Object.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }

            #endregion

            #region Intake Station 3

            GameObject intakeStation3Object = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(intakeStation3Object);
            intakeStation3Object.SetActive(true);
            intakeStation3Object.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station intakeStation3 = theFSM.FindStation("IntakeStation3");

            intakeStation3Object.transform.GetChild(0).GetComponent<Text>().text = "Intake Station 3";
            intakeStation3Object.transform.GetChild(3).GetComponent<Text>().text =
                "This station is the first step in processing the solids that are sent to the station. " +
                "It ensures the solid is something that the station is capable of handling and sends it to the next station.";
            intakeStation3Object.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + intakeStation3.speed + " seconds";
            if (intakeStation3.queue.Count > 0)
            {
                intakeStation3Object.transform.GetChild(6).GetComponent<Text>().text = intakeStation3.queue.Peek().capsuleGUID;
            }
            else
            {
                intakeStation3Object.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            intakeStation3Object.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + intakeStation3.queue.Count + "): ";

            if (intakeStation3.queue.Count > 1)
            {
                foreach (Capsule theCapsule in intakeStation3.queue)
                {
                    intakeStation3Object.transform.GetChild(8).GetComponent<Text>().text =
                        intakeStation3Object.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                intakeStation3Object.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }

            #endregion

            #region Intake Station 4

            GameObject intakeStation4Object = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(intakeStation4Object);
            intakeStation4Object.SetActive(true);
            intakeStation4Object.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station intakeStation4 = theFSM.FindStation("IntakeStation4");

            intakeStation4Object.transform.GetChild(0).GetComponent<Text>().text = "Intake Station 4";
            intakeStation4Object.transform.GetChild(3).GetComponent<Text>().text =
                "This station is the first step in processing the solids that are sent to the station. " +
                "It ensures the solid is something that the station is capable of handling and sends it to the next station.";
            intakeStation4Object.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + intakeStation4.speed + " seconds";
            if (intakeStation4.queue.Count > 0)
            {
                intakeStation4Object.transform.GetChild(6).GetComponent<Text>().text = intakeStation4.queue.Peek().capsuleGUID;
            }
            else
            {
                intakeStation4Object.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            intakeStation4Object.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + intakeStation4.queue.Count + "): ";

            if (intakeStation4.queue.Count > 1)
            {
                foreach (Capsule theCapsule in intakeStation4.queue)
                {
                    intakeStation4Object.transform.GetChild(8).GetComponent<Text>().text =
                        intakeStation4Object.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                intakeStation4Object.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }

            #endregion

            #region Intake Station 5

            GameObject intakeStation5Object = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(intakeStation5Object);
            intakeStation5Object.SetActive(true);
            intakeStation5Object.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station intakeStation5 = theFSM.FindStation("IntakeStation5");

            intakeStation5Object.transform.GetChild(0).GetComponent<Text>().text = "Intake Station 5";
            intakeStation5Object.transform.GetChild(3).GetComponent<Text>().text =
                "This station is the first step in processing the solids that are sent to the station. " +
                "It ensures the solid is something that the station is capable of handling and sends it to the next station.";
            intakeStation5Object.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + intakeStation5.speed + " seconds";
            if (intakeStation5.queue.Count > 0)
            {
                intakeStation5Object.transform.GetChild(6).GetComponent<Text>().text = intakeStation5.queue.Peek().capsuleGUID;
            }
            else
            {
                intakeStation5Object.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            intakeStation5Object.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + intakeStation5.queue.Count + "): ";

            if (intakeStation5.queue.Count > 1)
            {
                foreach (Capsule theCapsule in intakeStation5.queue)
                {
                    intakeStation5Object.transform.GetChild(8).GetComponent<Text>().text =
                        intakeStation5Object.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                intakeStation5Object.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }

            #endregion

            #region Intake Station 6

            GameObject intakeStation6Object = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(intakeStation6Object);
            intakeStation6Object.SetActive(true);
            intakeStation6Object.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station intakeStation6 = theFSM.FindStation("IntakeStation6");

            intakeStation6Object.transform.GetChild(0).GetComponent<Text>().text = "Intake Station 6";
            intakeStation6Object.transform.GetChild(3).GetComponent<Text>().text =
                " This station is the first step in processing the gases that are sent to the station. " +
                "It ensures the gas is something that the station is capable of handling and sends it to the next station.";
            intakeStation6Object.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + intakeStation6.speed + " seconds";
            if (intakeStation6.queue.Count > 0)
            {
                intakeStation6Object.transform.GetChild(6).GetComponent<Text>().text = intakeStation6.queue.Peek().capsuleGUID;
            }
            else
            {
                intakeStation6Object.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            intakeStation6Object.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + intakeStation6.queue.Count + "): ";

            if (intakeStation6.queue.Count > 1)
            {
                foreach (Capsule theCapsule in intakeStation6.queue)
                {
                    intakeStation6Object.transform.GetChild(8).GetComponent<Text>().text =
                        intakeStation6Object.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                intakeStation6Object.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }

            #endregion
        }

        if (stationName.ToLower().Contains(("weighing")))
        {
            #region Weighing Station 1

            GameObject weighingStation1Object = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(weighingStation1Object);
            weighingStation1Object.SetActive(true);
            weighingStation1Object.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station weighingStation1 = theFSM.FindStation("WeighingStation1");

            weighingStation1Object.transform.GetChild(0).GetComponent<Text>().text = "Fluids Weighing Station";
            weighingStation1Object.transform.GetChild(3).GetComponent<Text>().text =
                "This station is responsible for weighing all the fluids (gases and liquids) that come into the station." + 
                " It takes in the objects from intake stations 1 and 2 and releases them to be inspected.";
            weighingStation1Object.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + weighingStation1.speed + " seconds";
            if (weighingStation1.queue.Count > 0)
            {
                weighingStation1Object.transform.GetChild(6).GetComponent<Text>().text = weighingStation1.queue.Peek().capsuleGUID;
            }
            else
            {
                weighingStation1Object.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            weighingStation1Object.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + weighingStation1.queue.Count + "): ";

            if (weighingStation1.queue.Count > 1)
            {
                foreach (Capsule theCapsule in weighingStation1.queue)
                {
                    weighingStation1Object.transform.GetChild(8).GetComponent<Text>().text =
                        weighingStation1Object.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                weighingStation1Object.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }

            #endregion

            #region Weighing Station 2

            GameObject weighingStation2Object = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(weighingStation2Object);
            weighingStation2Object.SetActive(true);
            weighingStation2Object.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station weighingStation2 = theFSM.FindStation("WeighingStation2");

            weighingStation2Object.transform.GetChild(0).GetComponent<Text>().text = "Solids Weighing Station";
            weighingStation2Object.transform.GetChild(3).GetComponent<Text>().text =
                "This station is responsible for weighing all the solid materials that come into the station." +
                " It takes in the objects from intake stations 3 and 4 and releases them to be inspected.";
            weighingStation2Object.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + weighingStation2.speed + " seconds";
            if (weighingStation2.queue.Count > 0)
            {
                weighingStation2Object.transform.GetChild(6).GetComponent<Text>().text = weighingStation2.queue.Peek().capsuleGUID;
            }
            else
            {
                weighingStation2Object.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            weighingStation2Object.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + weighingStation2.queue.Count + "): ";

            if (weighingStation2.queue.Count > 1)
            {
                foreach (Capsule theCapsule in weighingStation2.queue)
                {
                    weighingStation2Object.transform.GetChild(8).GetComponent<Text>().text =
                        weighingStation2Object.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                weighingStation2Object.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }

            #endregion

            #region Weighing Station 3

            GameObject weighingStation3Object = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(weighingStation3Object);
            weighingStation3Object.SetActive(true);
            weighingStation3Object.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station weighingStation3 = theFSM.FindStation("WeighingStation3");

            weighingStation3Object.transform.GetChild(0).GetComponent<Text>().text = "General Weighing Station";
            weighingStation3Object.transform.GetChild(3).GetComponent<Text>().text =
                "This station is responsible for weighing all the materials (gases, liquids and solids) that come into the station." +
                " It takes in the objects from intake stations 5 and 6 and releases them to be inspected.";
            weighingStation3Object.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + weighingStation3.speed + " seconds";
            if (weighingStation3.queue.Count > 0)
            {
                weighingStation3Object.transform.GetChild(6).GetComponent<Text>().text = weighingStation3.queue.Peek().capsuleGUID;
            }
            else
            {
                weighingStation3Object.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            weighingStation3Object.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + weighingStation3.queue.Count + "): ";

            if (weighingStation3.queue.Count > 1)
            {
                foreach (Capsule theCapsule in weighingStation3.queue)
                {
                    weighingStation3Object.transform.GetChild(8).GetComponent<Text>().text =
                        weighingStation3Object.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                weighingStation3Object.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }

            #endregion
        }

        if (stationName.ToLower().Contains(("packaging")))
        {
            GameObject packagingStationObject = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(packagingStationObject);
            packagingStationObject.SetActive(true);
            packagingStationObject.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station packagingStation = theFSM.FindStation("PackagingStation");

            packagingStationObject.transform.GetChild(0).GetComponent<Text>().text = "Packaging Station";
            packagingStationObject.transform.GetChild(3).GetComponent<Text>().text =
                "This station is responsible for adding a tracking microchip and packaging all the materials for the later " +
                "stages. All 3 weighing stations converge to one packaging station as only one packaging station is available.";
            packagingStationObject.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + packagingStation.speed + " seconds";
            if (packagingStation.queue.Count > 0)
            {
                packagingStationObject.transform.GetChild(6).GetComponent<Text>().text = packagingStation.queue.Peek().capsuleGUID;
            }
            else
            {
                packagingStationObject.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            packagingStationObject.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + packagingStation.queue.Count + "): ";

            if (packagingStation.queue.Count > 1)
            {
                foreach (Capsule theCapsule in packagingStation.queue)
                {
                    packagingStationObject.transform.GetChild(8).GetComponent<Text>().text =
                        packagingStationObject.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                packagingStationObject.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }
        }

        if (stationName.ToLower().Contains(("inspection")))
        {
            GameObject inspectionStationObject = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(inspectionStationObject);
            inspectionStationObject.SetActive(true);
            inspectionStationObject.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station inspectionStation = theFSM.FindStation("InspectionStation");

            inspectionStationObject.transform.GetChild(0).GetComponent<Text>().text = "Inspection Station";
            inspectionStationObject.transform.GetChild(3).GetComponent<Text>().text =
                "All capsules must undergo inspection in this station to ensure that the capsule doesn't contain any foreign materials " +
                "or any other defects that may exist within the capsule.";
            inspectionStationObject.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + inspectionStation.speed + " seconds";
            if (inspectionStation.queue.Count > 0)
            {
                inspectionStationObject.transform.GetChild(6).GetComponent<Text>().text = inspectionStation.queue.Peek().capsuleGUID;
            }
            else
            {
                inspectionStationObject.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            inspectionStationObject.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + inspectionStation.queue.Count + "): ";

            if (inspectionStation.queue.Count > 1)
            {
                foreach (Capsule theCapsule in inspectionStation.queue)
                {
                    inspectionStationObject.transform.GetChild(8).GetComponent<Text>().text =
                        inspectionStationObject.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                inspectionStationObject.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }
        }

        if (stationName.ToLower().Contains(("processing")))
        {
            GameObject processingStationObject = Instantiate(stationInfoPanelTemplate) as GameObject;
            stationInfoObjects.Add(processingStationObject);
            processingStationObject.SetActive(true);
            processingStationObject.transform.SetParent(stationInfoPanelTemplate.transform.parent, false);
            Station processingStation = theFSM.FindStation("ProcessingStation");

            processingStationObject.transform.GetChild(0).GetComponent<Text>().text = "Processing Station";
            processingStationObject.transform.GetChild(3).GetComponent<Text>().text =
                "The processing station is responsible for processing any fluids that enter to ensure they remain at the " +
                "same temperature for later stages. Solids don't need to be processed but go through anyway.";
            processingStationObject.transform.GetChild(4).GetComponent<Text>().text = "Processing Speed: " + processingStation.speed + " seconds";

            if (processingStation.queue.Count > 0)
            {
                processingStationObject.transform.GetChild(6).GetComponent<Text>().text = processingStation.queue.Peek().capsuleGUID;
            }
            else
            {
                processingStationObject.transform.GetChild(6).GetComponent<Text>().text = "not processing anything";
            }

            processingStationObject.transform.GetChild(7).GetComponent<Text>().text = "Capsule Queue(" + processingStation.queue.Count + "): "; 

            if (processingStation.queue.Count > 1)
            {
                foreach (Capsule theCapsule in processingStation.queue)
                {
                    processingStationObject.transform.GetChild(8).GetComponent<Text>().text =
                        processingStationObject.transform.GetChild(8).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
                }
            }
            else
            {
                processingStationObject.transform.GetChild(8).GetComponent<Text>().text = "nothing in queue";
            }
        }
    }

    public void OpenStorageInfoMenu(string storageName)
    {
        CloseGameMenus();

        foreach (GameObject panel in infoPanels)
        {
            Destroy(panel);
        }

        GameObject storageInfoMenu = gameMenuCanvas.transform.GetChild(5).gameObject;
        storageInfoMenu.SetActive(true);

        infoPanels.Add(CreateStorageInfoPanel(theFSM.FindStorage("SolidStorage"), "Solid Storage"));
        infoPanels.Add(CreateStorageInfoPanel(theFSM.FindStorage("LiquidStorage"), "Liquid Storage"));
        infoPanels.Add(CreateStorageInfoPanel(theFSM.FindStorage("GasStorage"), "Gas Storage"));
    }

    public GameObject CreateStorageInfoPanel(Storage theStorage, string theName)
    {
        GameObject storageObject = Instantiate(storageInfoPanelTemplate) as GameObject;
        storageObject.SetActive(true);
        storageObject.transform.SetParent(storageInfoPanelTemplate.transform.parent, false);

        storageObject.transform.GetChild(0).GetComponent<Text>().text = theName;

        storageObject.transform.GetChild(2).GetComponent<Text>().text = "Capacity: " + storageCapacity;

        storageObject.transform.GetChild(3).GetComponent<Text>().text = "Capsule Queue(" + theStorage.queue.Count + "): ";

        if (theStorage.queue.Count > 0)
        {
            foreach (Capsule theCapsule in theStorage.queue)
            {
                storageObject.transform.GetChild(4).GetComponent<Text>().text =
                    storageObject.transform.GetChild(4).GetComponent<Text>().text + theCapsule.capsuleGUID + "\n";
            }
        }
        else
        {
            storageObject.transform.GetChild(4).GetComponent<Text>().text = "nothing is currently stored";
        }

        return storageObject;
    }

    public void OpenCapsuleInfoMenu()
    {
        CloseGameMenus();
        
        GameObject capsuleMenu = gameMenuCanvas.transform.GetChild(6).gameObject;
        capsuleMenu.SetActive(true);

        foreach (GameObject panel in infoPanels)
        {
            Destroy(panel);
        }

        string activeDescription = "Active capsules consist of every capsule that is currently still being processed in the spaceport";
        string exitedDescription = "Exited capsules consist of every capsule that has already been processed and exited the spaceport";

        infoPanels.Add(CreateCapsulesInfoPanel(theFSM.activeCapsules, "Active Capsules", activeDescription));
        infoPanels.Add(CreateCapsulesInfoPanel(theFSM.exitedCapsules, "Exited Capsules", exitedDescription));
    }

    public GameObject CreateCapsulesInfoPanel(List<Capsule> theCapsulesList, string theName, string theDescription)
    {
        GameObject capsuleInfoObject = Instantiate(capsuleInfoPanelTemplate) as GameObject;
        capsuleInfoObject.SetActive(true);
        capsuleInfoObject.transform.SetParent(capsuleInfoPanelTemplate.transform.parent, false);

        capsuleInfoObject.transform.GetChild(0).GetComponent<Text>().text = theName;

        capsuleInfoObject.transform.GetChild(2).GetComponent<Text>().text = theDescription;

        capsuleInfoObject.transform.GetChild(3).GetComponent<Text>().text = "Amount: " + theCapsulesList.Count;

        return capsuleInfoObject;
    }

    public void GenerateStationsButtons()
    {
        if (theFSM.activeStations != null)
        {
            foreach (Station theStation in theFSM.activeStations)
            {
                GameObject button = Instantiate(buttonTemplate) as GameObject;
                button.SetActive(true);
                StationsListButton buttonComponent = button.GetComponent<StationsListButton>();

                if (buttonComponent != null)
                {
                    buttonComponent.SetText(theStation.name);
                }
                else
                {
                    Debug.Log("button component not found in GenerateButtons");
                }

                button.transform.SetParent(buttonTemplate.transform.parent, false);
            }
        }
        else
        {
            Debug.Log("Stations not found in GenerateButtons");
        }
    }

    public void UpdateWorldCanvasInfo()
    {
        UpdateWeighingCanvas();
        UpdatePackagingCanvas();
        UpdateInspectionCanvas();
        UpdateProcessingCanvas();
        UpdateStorageCanvas();
    }

    public void UpdateWeighingCanvas()
    {
        Station weighing1 = theFSM.FindStation("WeighingStation1");
        Station weighing2 = theFSM.FindStation("WeighingStation2");
        Station weighing3 = theFSM.FindStation("WeighingStation3");

        worldCanvases.transform.GetChild(2).transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text =
            "All Weighing Station: " + weighing1.queue.Count +
            "\n\nSolids Weighing Station: " + weighing2.queue.Count +
            "\n\nFluids Weighing Station: " + weighing3.queue.Count;
    }

    public void UpdatePackagingCanvas()
    {
        Station packaging = theFSM.FindStation("PackagingStation");

        string inUse = "no";

        if (packaging.queue.Count > 0)
        {
            inUse = "yes";
        }

        worldCanvases.transform.GetChild(3).transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text =
            "In Use: " + inUse + "\n\nQueue: " + packaging.queue.Count;
    }

    public void UpdateInspectionCanvas()
    {
        Station inspection = theFSM.FindStation("InspectionStation");

        string inUse = "no";

        if (inspection.queue.Count > 0)
        {
            inUse = "yes";
        }

        worldCanvases.transform.GetChild(4).transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text =
            "In Use: " + inUse + "\n\nQueue: " + inspection.queue.Count;
    }

    public void UpdateProcessingCanvas()
    {
        Station inspection = theFSM.FindStation("ProcessingStation");

        string inUse = "no";

        if (inspection.queue.Count > 0)
        {
            inUse = "yes";
        }

        worldCanvases.transform.GetChild(5).transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text =
            "In Use: " + inUse + "\n\nQueue: " + inspection.queue.Count;
    }

    public void UpdateStorageCanvas()
    {
        Storage solidStorage = theFSM.FindStorage("SolidStorage");
        Storage liquidStorage = theFSM.FindStorage("LiquidStorage");
        Storage gasStorage = theFSM.FindStorage("GasStorage");

        worldCanvases.transform.GetChild(6).transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text =
            "Solid Capsules: " + solidStorage.queue.Count +
            "\n\nLiquid Capsules: " + liquidStorage.queue.Count +
            "\n\nGas Capsules: " + gasStorage.queue.Count;
    }

    public void OpenWebsite()
    {
        Application.OpenURL("http://i4competition.ca");
    }

    public void SetVolume(float theVolume)
    {
        audioMixer.SetFloat("volume", theVolume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void CreateResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> optionsList = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            optionsList.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(optionsList);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution theResolution = resolutions[resolutionIndex];
        Screen.SetResolution(theResolution.width, theResolution.height, Screen.fullScreen);
    }
}
