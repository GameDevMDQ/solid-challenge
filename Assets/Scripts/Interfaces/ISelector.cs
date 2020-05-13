using UnityEngine;

public interface ISelector
{
    CoinBehavior Check(Ray ray);
}