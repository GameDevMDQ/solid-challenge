using UnityEngine;

public class ExplodingCoin : CoinBehavior
{
	[SerializeField] private ParticleSystem _explosionEffect = null;
	
	private const int ScoreForCollection = 50;
	
	public override void Collect()
	{
		ScoreManager.Score += ScoreForCollection;
		
		Explode();
		
		Destroy(gameObject);
	}

	private void Explode()
	{
		ParticleSystem createdVfx = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
		Destroy(createdVfx.gameObject, createdVfx.main.duration);
	}
}
