using UnityEngine;
using System.Collections;

public class Generator {

    private ParticleManager _pm;

    public Generator(ParticleManager _pm)
    {
        this._pm = _pm;
    }

    public void Generate()
    {
        // _pm.ConstantLife = Random.Range(0, 2) == 0 ? true : false;
        if (_pm.GenerateLife)
        {
            _pm.MinLife = Random.Range(.5f, 1.0f) * _pm.LifeSlider;
            _pm.MaxLife = Random.Range(_pm.MinLife, _pm.MinLife + 1.5f) * _pm.LifeSlider;
            _pm.Life = Random.Range(.3f, 2.0f) * _pm.LifeSlider;
        }

        if (_pm.GenerateSpeed)
        {
            _pm.Speed = Random.Range(150, 200) * _pm.SpeedSlider;
            _pm.MinSpeed = Random.Range(150, 240.0f) * _pm.SpeedSlider;
            _pm.MaxSpeed = Random.Range(_pm.MinSpeed, _pm.MinSpeed + 140.0f) * _pm.SpeedSlider;

        }

        if (_pm.GenerateSize)
        {
            _pm.Size = Random.Range(.1f, 4.0f) * _pm.SizeSlider;
            _pm.MinSize = Random.Range(1, 3.0f) * _pm.SizeSlider;
            _pm.MaxSize = Random.Range(_pm.MinSize, _pm.MinSize + 5.0f) * _pm.SizeSlider;

            _pm.ParticleSize = GenerateAnimationCurve();
        }

        if (_pm.GenerateColor)
        {

            if(_pm.ColorOverLifeTime == false)
            {
                _pm.Color = GenerateColor();
            }
            else
            {
                _pm.Color = Color.white;       
            }
            _pm.ColorGradient = GenerateColorGradient();
            
        }

        if (_pm.GenerateAngle)
        {
            _pm.Angle = Random.Range(0.0f, 4.0f) * _pm.AngleSlider; 
        }

        if (_pm.GenerateRadius)
        {
            _pm.Radius = Random.Range(0.0f, _pm.Angle + 1.0f) * _pm.RadiusSlider; 
        }

        if (_pm.GenerateIntensity)
        {
            _pm.Intensity = Random.Range(1000, 1500) * _pm.IntensitySlider; 
        }

        // _pm.ConstantSpeed = Random.Range(0, 2) == 0 ? true : false;
    }

    private AnimationCurve GenerateAnimationCurve()
    {
        AnimationCurve curve = new AnimationCurve();
        int keys = Random.Range(4, 5);
        for (int i = 0; i < keys; i++)
        {
            Keyframe keyFrame = new Keyframe(Random.Range(0.0f, 1.0f) * _pm.SizeSlider, Random.Range(1.0f, 10.0f) + 2.0f * _pm.SizeSlider);
            curve.AddKey(keyFrame);
        }

        return curve;
    }

    private Color GenerateColor()
    {
        Color color = new Color();
        color.r = Random.Range(0.0f, 1.0f);
        color.g = Random.Range(0.0f, 1.0f);
        color.b = Random.Range(0.0f, 1.0f);
        color.a = Random.Range(0.0f, 1.0f);

        return color; 
    }

    private Gradient GenerateColorGradient()
    {
        Gradient gradient = new Gradient();
        int colorKeys = Random.Range(2, 6); 
        GradientColorKey[] gradientColorkey = new GradientColorKey[colorKeys];
        GradientAlphaKey[] gradientAlphakey = new GradientAlphaKey[colorKeys / 2]; 
        for(int i = 0;i < colorKeys; i++)
        {
            gradientColorkey[i] = new GradientColorKey(GenerateColor(), Random.Range(0.0f, 1.0f));     
        }

        for(int i = 0;i < gradientAlphakey.Length; i++)
        {
            gradientAlphakey[i] = new GradientAlphaKey(1.0f,Random.Range(0.0f, 1.0f)); 
        }

        gradient.SetKeys(gradientColorkey, gradientAlphakey); 
        return gradient; 
    }
}
