using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {
    Roam,
    Chase, // When enemy is close
    Evade  // When enemy is in EAT mode
}