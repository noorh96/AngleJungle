using UnityEngine;
using System.Collections;

public class LaserScriptableObject : ScriptableObject
{
    public Transform t; 
    public Vector3 position;
    public Quaternion rotation;  

    public Material laserMaterial;


    public float Life, MinLife, MaxLife;
    public bool ConstantLife;

    public float Speed, MinSpeed, MaxSpeed;
    public bool ConstantSpeed;

    public float Size, MinSize, MaxSize;
    public bool ConstantSize;

    public float Intensity;

    public float Gravity;

    public float Angle, Radius;

    public Color Color;

    public AnimationCurve ParticleSize;

    public Gradient ColorGradient;

    public bool SizeOverLifeTime, ColorOverLifeTime, EnableCollision;

    public bool GenerateLaser , GenerateLife, GenerateSpeed, GenerateSize, GenerateColor, GenerateAngle, GenerateRadius, GenerateIntensity;
    public float GenerationScale;

    public ParticleSystem.EmissionModule emmision;
    public ParticleSystem.ShapeModule shape;
    public ParticleSystem.SizeOverLifetimeModule sizeOverLifeTime;
    public ParticleSystem.ColorOverLifetimeModule colorOverLifeTime;
    public ParticleSystem.CollisionModule collision;

    [Range(0.0F, 1.0F)]
    public float Dampen = 0;
    [Range(0.0f, 1.0f)]
    public float LifetimeLoss = 0;
    [Range(0.0f, 1.0f)]
    public float Bounce = 0;

    [Range(0.0F, 1.0F)]
    public float LifeSlider = 0;
    [Range(0.0F, 1.0F)]
    public float SpeedSlider = 0;
    [Range(0.0F, 1.0F)]
    public float SizeSlider = 0;
    [Range(0.0F, 1.0F)]
    public float AngleSlider = 0;
    [Range(0.0F, 1.0F)]
    public float RadiusSlider = 0;
    [Range(0.0F, 1.0F)]
    public float IntensitySlider = 0;

    [SerializeField]
    public CollisionEnter OnLaserParticleCollision;

    public LayerMask CollidesWith;

    public int LifeOptionIndex, SpeedOptionIndex, SizeOptionIndex; 


    public void init(ParticleManager pm)
    {
        position = pm.transform.position;
        rotation = pm.transform.rotation;

        laserMaterial = pm.laserMaterial;

        Life = pm.Life;
        MinLife = pm.MinLife;
        MaxLife = pm.MaxLife;
        ConstantLife = pm.ConstantLife;

        Speed = pm.Speed;
        MinSpeed = pm.MinSpeed;
        MaxSpeed = pm.MaxSpeed;
        ConstantSpeed = pm.ConstantSpeed;

        Size = pm.Size;
        MinSize = pm.MinSize;
        MaxSize = pm.MaxSize;
        ConstantSize = pm.ConstantSize;
        ParticleSize = pm.ParticleSize;
        SizeOverLifeTime = pm.SizeOverLifeTime;

        Intensity = pm.Intensity;
        Gravity = pm.Gravity;
        Angle = pm.Angle;
        Radius = pm.Radius;

        Color = pm.Color;
        ColorGradient = pm.ColorGradient;
        ColorOverLifeTime = pm.ColorOverLifeTime;
        EnableCollision = pm.EnableCollision;

        emmision = pm.emmision;
        shape = pm.shape;
        sizeOverLifeTime = pm.sizeOverLifeTime;
        colorOverLifeTime = pm.colorOverLifeTime;
        collision = pm.collision;
        CollidesWith = pm.CollidesWith;

        Dampen = pm.Dampen;
        LifetimeLoss = pm.LifetimeLoss;
        Bounce = pm.Bounce;


        GenerateLaser = pm.GenerateLaser;
        GenerateLife = pm.GenerateLife;
        GenerateSpeed = pm.GenerateSpeed;
        GenerateSize = pm.GenerateSize;
        GenerateColor = pm.GenerateColor;
        GenerateAngle = pm.GenerateAngle;
        GenerateRadius = pm.GenerateRadius;
        GenerateIntensity = pm.GenerateIntensity;
        GenerationScale = pm.GenerationScale;

        LifeSlider = pm.LifeSlider;
        SpeedSlider = pm.SpeedSlider;
        SizeSlider = pm.SizeSlider;
        AngleSlider = pm.AngleSlider;
        RadiusSlider = pm.RadiusSlider;
        IntensitySlider = pm.IntensitySlider;  

        LifeOptionIndex = pm.LifeOptionIndex; 
        SpeedOptionIndex = pm.SpeedOptionIndex; 
        SizeOptionIndex = pm.SizeOptionIndex; 


        //OnLaserParticleCollision = pm.OnLaserParticleCollision;
    }


 

    public static LaserScriptableObject CreateInstance(ParticleManager pm)
    {
        var data = ScriptableObject.CreateInstance<LaserScriptableObject>();
        data.init(pm); 
        return data; 
    }
}
