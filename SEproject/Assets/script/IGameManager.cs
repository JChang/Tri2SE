using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager : ICarObserver
{
    public void AddPoints(float points);
    public void SpawnCar();
    public IEnumerator SpawnCarsAtInterval();
    public Transform GetCameraTransform();
}
