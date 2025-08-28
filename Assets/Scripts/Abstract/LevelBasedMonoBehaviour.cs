using UnityEngine;

namespace Abstract
{
    public abstract class LevelBasedMonoBehaviour : MonoBehaviour
    {
        [SerializeField] protected LevelDatabaseSo levelDatabase;
        [SerializeField] protected LevelDataSo currentLevel;

        protected virtual void InitializeLevelSettings()
        {
            // int levelIndex = PlayerPrefs.GetInt("level", 0);
            

            int levelIndex = LevelManager.Instance.GetCurrentLevelIndex();
            currentLevel = levelDatabase.levels[Mathf.Clamp(levelIndex, 0, levelDatabase.levels.Count - 1)];
        }
    }
}