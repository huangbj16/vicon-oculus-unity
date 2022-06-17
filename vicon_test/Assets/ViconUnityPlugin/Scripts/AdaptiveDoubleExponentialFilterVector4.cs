using UnityEngine;

public class AdaptiveDoubleExponentialFilterVector4
{
	private AdaptiveDoubleExponentialFilterFloat x;
	private AdaptiveDoubleExponentialFilterFloat y;
	private AdaptiveDoubleExponentialFilterFloat z;
	private AdaptiveDoubleExponentialFilterFloat w;

	public Vector4 Value
	{
		get { return new Vector4(x.Value, y.Value, z.Value, w.Value); }
		set { Update(value); }
	}

	public AdaptiveDoubleExponentialFilterVector4()
	{
		x = new AdaptiveDoubleExponentialFilterFloat();
		y = new AdaptiveDoubleExponentialFilterFloat();
		z = new AdaptiveDoubleExponentialFilterFloat();
		w = new AdaptiveDoubleExponentialFilterFloat();
	}

	private void Update(Vector4 v)
	{
		x.Value = v.x;
		y.Value = v.y;
		z.Value = v.z;
		w.Value = v.w;
	}
}