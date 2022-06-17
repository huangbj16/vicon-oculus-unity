using UnityEngine;

public class KalmanFilterVector4
{
	private KalmanFilterFloat _x;
	private KalmanFilterFloat _y;
	private KalmanFilterFloat _z;
	private KalmanFilterFloat _w;

	public Vector4 Value
	{
		get
		{
			return new Vector4(_x.Value, _y.Value, _z.Value, _w.Value);
		}
		set
		{
			_update(value);
		}
	}

	public KalmanFilterVector4()
	{
		_x = new KalmanFilterFloat();
		_y = new KalmanFilterFloat();
		_z = new KalmanFilterFloat();
		_w = new KalmanFilterFloat();
	}

	private void _update(Vector4 v)
	{
		_x.Value = v.x;
		_y.Value = v.y;
		_z.Value = v.z;
		_w.Value = v.w;
	}
}