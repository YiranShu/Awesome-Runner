using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    private int levelLength;

    [SerializeField]
    private int startLevelLength = 5;

    [SerializeField]
    private int endLevelLength = 5;

    [SerializeField]
    private int distanceBetweenPlatforms;

    [SerializeField]
    private Transform platformPrefabs, platformParent;

    [SerializeField]
    private Transform monster, monsterParent;

    [SerializeField]
    private Transform healthCollectable, healthCollectableParent;

    [SerializeField]
    private float platformPosition_MinY = 0f, platformPosition_MaxY = 10f;

    [SerializeField]
    private int platformLength_Min = 1, platformLength_Max = 4;

    [SerializeField]
    private float chanceForMonsterExistence = 0.25f, chanceForCollectableExistence = 0.1f;

    [SerializeField]
    private float healthCollectable_MinY = 1f, healthCollectable_MaxY = 3f;

    private float platformLastPositionX;

    private enum PlatformType {
        None,
        Flat
    }

    private class PlatformPositionInfo {
        public PlatformType platformType;
        public float positionY;
        public bool hasMonster;
        public bool hasHealthCollectable;

        public PlatformPositionInfo(PlatformType type, float positionY, bool hasMonster, bool hasHealthCollectable) {
            this.platformType = type;
            this.positionY = positionY;
            this.hasMonster = hasMonster;
            this.hasHealthCollectable = hasHealthCollectable;
        }
    }

    void Start() {
        GenerateLevel(true);
    }

    void FillOutPositionInfo(PlatformPositionInfo[] platformInfo) {
        int currentPlatformInfoIndex = 0;

        for(int i = 0; i < startLevelLength; i++) {
            platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
            platformInfo[currentPlatformInfoIndex].positionY = 0f;

            currentPlatformInfoIndex++;
        }

        while(currentPlatformInfoIndex < levelLength - endLevelLength) {
            if(platformInfo[currentPlatformInfoIndex - 1].platformType != PlatformType.None) {
                currentPlatformInfoIndex++;
                continue;
            }

            float platformPositonY = Random.Range(platformPosition_MinY, platformPosition_MaxY);
            int platformLength = Random.Range(platformLength_Min, platformLength_Max);

            for(int i = 0; i < platformLength; i++) {
                bool has_Monster = (Random.Range(0f, 1f) < chanceForMonsterExistence);
                bool has_HealthCollectable = (Random.Range(0f, 1f) < chanceForCollectableExistence);

                platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = platformPositonY;
                platformInfo[currentPlatformInfoIndex].hasMonster = has_Monster;
                platformInfo[currentPlatformInfoIndex].hasHealthCollectable = has_HealthCollectable;

                currentPlatformInfoIndex++;

                if(currentPlatformInfoIndex > (levelLength - endLevelLength)) {
                    currentPlatformInfoIndex = levelLength - endLevelLength;
                    break;
                }

            }

            for(int i = 0; i < endLevelLength; i++) {
                platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = 0f;

                currentPlatformInfoIndex++;
            } // Maybe put out of the while loop!!!
        }
    }

    void CreatePlatformsFromPositionInfo(PlatformPositionInfo[] platformPositionInfo, bool gameStarted) {
        for(int i = 0; i < platformPositionInfo.Length; i++) {
            PlatformPositionInfo positionInfo = platformPositionInfo[i];

            if(positionInfo.platformType == PlatformType.None) {
                continue;
            }

            Vector3 platformPosition;

            // Here we are going to check if the game is started or not
            if (gameStarted) {
                platformPosition = new Vector3(distanceBetweenPlatforms * i, positionInfo.positionY, 0f);
            } else {
                platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX, positionInfo.positionY, 0f);
            }

            // save the platform postion x for later use
            platformLastPositionX = platformPosition.x;

            Transform createBlock = (Transform)Instantiate(platformPrefabs, platformPosition, Quaternion.identity);
            createBlock.parent = platformParent;

            if(positionInfo.hasMonster) {
                //set monster
                if (gameStarted) { 
                    platformPosition = new Vector3(distanceBetweenPlatforms * i, positionInfo.positionY + 0.1f, 0f);
                } else {
                    platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX, positionInfo.positionY + 0.1f, 0f);
                }

                Transform createMonster = (Transform)Instantiate(monster, platformPosition, Quaternion.Euler(0f, -90f, 0f));
                createMonster.parent = monsterParent;
            }

            if(positionInfo.hasHealthCollectable) {
                //set health
                if(gameStarted) {
                    platformPosition = new Vector3(distanceBetweenPlatforms * i,
                        positionInfo.positionY + Random.Range(healthCollectable_MinY, healthCollectable_MaxY), 0f);
                } else {
                    platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX,
                        positionInfo.positionY + Random.Range(healthCollectable_MinY, healthCollectable_MaxY), 0f);
                }

                Transform createHealth = (Transform)Instantiate(healthCollectable, platformPosition, Quaternion.identity);
                createHealth.parent = healthCollectableParent;
            }
        }
    }

    public void GenerateLevel(bool gameStarted) {
        PlatformPositionInfo[] platformInfo = new PlatformPositionInfo[levelLength];

        for(int i = 0; i < platformInfo.Length; i++) {
            platformInfo[i] = new PlatformPositionInfo(PlatformType.None, -1f, false, false);
        }

        FillOutPositionInfo(platformInfo);
        CreatePlatformsFromPositionInfo(platformInfo, gameStarted);
    }
}
