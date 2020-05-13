using UnityEngine;

public class Coin : CoinBehavior
{
	private const int ScoreForCollection = 10;
	
	public override void Collect()
	{
		ScoreManager.Score += ScoreForCollection;
		
		Destroy(gameObject);
	}
}
