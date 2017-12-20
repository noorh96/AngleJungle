using UnityEngine;
using System.Collections;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Events;
using UnityEditor;
#endif

using System.IO;
using System;
[ExecuteInEditMode]
public class ParticleManager : MonoBehaviour
{

    ParticleSystem _ps;
    private LaserScriptableObject lso;
    #region------PUBLIC MEMEBERS-----

    public Material laserMaterial;
    public GameObject thisObject;
    public bool shouldPersist;
    public bool didApplicationQuit;

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
    public bool GenerateLaser, GenerateLife, GenerateSpeed, GenerateSize, GenerateColor, GenerateAngle, GenerateRadius, GenerateIntensity;
    public float GenerationScale = 1;

    #endregion------/PUBLIC MEMEBERS-----


    #region---- PRIVATE MEMEBERS-----
    private Material _material;
    private Color tempColor;
    private ParticleSystemRenderer _pr;


    private ParticleSystem.MinMaxCurve dampenModifiers = new ParticleSystem.MinMaxCurve();
    private ParticleSystem.MinMaxCurve lifeTimeLossModifiers = new ParticleSystem.MinMaxCurve();
    private ParticleSystem.MinMaxCurve bounceModifiers = new ParticleSystem.MinMaxCurve();
    private ParticleSystem.MinMaxCurve sizeOverLifeTimeCurve = new ParticleSystem.MinMaxCurve();
    private Generator generator;
    private bool callOnce;
    private bool skip;

    public int LifeOptionIndex, SpeedOptionIndex, SizeOptionIndex;

    #endregion-----/PRIVATE MEMBERS-----



    void Awake()
    {


        thisObject = transform.gameObject;

        if (gameObject.GetComponent<ParticleSystem>() == null)
        {
            gameObject.AddComponent<ParticleSystem>();
        }
        GenerationScale = 1;
        CreateScriptiableObjectForInstance();
        AssignValuesToInstance();

    }


    void CreateScriptiableObjectForInstance()
    {
#if UNITY_EDITOR
        LaserScriptableObject ll;
        if (File.Exists(Application.dataPath + "/VayLaserPack/Resources/ScriptableObjects/" + gameObject.name + "," + gameObject.GetHashCode() + ".asset") == false)
        {

            ll = LaserScriptableObject.CreateInstance(this);
            ll.name = gameObject.name;
            AssetDatabase.CreateAsset(ll, "Assets/VayLaserPack/Resources/ScriptableObjects/" + gameObject.name + "," + gameObject.GetHashCode() + ".asset");
        }
        else
        {
            ll = Resources.Load("ScriptableObjects/" + gameObject.name + "," + gameObject.GetHashCode()) as LaserScriptableObject;
        }

        lso = ll;
        AssetDatabase.Refresh();
#endif

    }


    void Start()
    {
        if (Resources.Load("LaserMaterial/_laser") != null && laserMaterial == null)
        {
            laserMaterial = (Material)Resources.Load("LaserMaterial/_laser");
        }

        _ps = GetComponent<ParticleSystem>();

        _pr = (ParticleSystemRenderer)GetComponent<Renderer>();
        Intensity = 1000;
        emmision = _ps.emission;
        emmision.enabled = true;
        shape = _ps.shape;
        sizeOverLifeTime = _ps.sizeOverLifetime;
        colorOverLifeTime = _ps.colorOverLifetime;
        collision = _ps.collision;
        _material = GetComponent<ParticleSystemRenderer>().sharedMaterial;
        _pr.material = laserMaterial;
        sizeOverLifeTimeCurve.mode = ParticleSystemCurveMode.Constant;
        generator = new Generator(this);

        tempColor = (Color.r == 0 && Color.g == 0 && Color.b == 0 && Color.a == 0) ? Color.white : Color;

        _ps.hideFlags = HideFlags.NotEditable;

        _ps.Play();
    }

    /// <summary>
    /// Resets the Color
    /// </summary>
    void ResetColor()
    {
        if (!callOnce)
        {
            Color = tempColor;
        }

        callOnce = true;
    }



    void Update()
    {

        AssignValuesToScriptableObject();
        SetBounds();
        _ps.maxParticles = (int)(Intensity * 20.5f);

        colorOverLifeTime.enabled = ColorOverLifeTime;

        collision.enabled = EnableCollision;
        _pr.material = laserMaterial;

        //Color
        if (ColorOverLifeTime)
        {
            colorOverLifeTime.color = new ParticleSystem.MinMaxGradient(ColorGradient);
            _ps.startColor = Color.white;
            if (!skip)
            {
                tempColor = Color;
                skip = true;
            }
            Color = Color.white;
            _material.SetColor("_TintColor", Color);
            callOnce = false;
        }
        else
        {
            skip = false;
            ResetColor();
            _ps.startColor = Color;

        }

        //Speed
        if (ConstantSpeed)
        {
            _ps.startSpeed = Speed;
        }
        else
        {
            _ps.startSpeed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
        }


        //Size
        if (ConstantSize)
        {
            _ps.startSize = Size;
        }
        else if (SizeOverLifeTime)
        {

            sizeOverLifeTimeCurve.constantMax = 10;

            sizeOverLifeTime.size = new ParticleSystem.MinMaxCurve(1, ParticleSize);

        }
        else
        {
            _ps.startSize = UnityEngine.Random.Range(MinSize, MaxSize);
        }


        if (ConstantLife)
        {
            _ps.startLifetime = Life;

        }
        else
        {
            _ps.startLifetime = UnityEngine.Random.Range(MinLife, MaxLife);

        }

        //Collision
        if (EnableCollision)
        {
            collision.type = ParticleSystemCollisionType.World;
            collision.mode = ParticleSystemCollisionMode.Collision3D;
            dampenModifiers.constantMax = Dampen;
            collision.dampen = dampenModifiers;
            lifeTimeLossModifiers.constantMax = LifetimeLoss;
            collision.lifetimeLoss = lifeTimeLossModifiers;
            bounceModifiers.constantMax = Bounce;
            collision.bounce = bounceModifiers;
            collision.collidesWith = CollidesWith;
            collision.sendCollisionMessages = true;
        }





        _ps.gravityModifier = Gravity;
        emmision.rate = new ParticleSystem.MinMaxCurve(Intensity);
        shape.angle = Angle;
        shape.radius = Radius;

        sizeOverLifeTime.enabled = SizeOverLifeTime;

    }

    void AssignValuesToInstance()
    {
#if UNITY_EDITOR
        transform.position = lso.position;
        transform.rotation = lso.rotation;

        laserMaterial = lso.laserMaterial;

        Life = lso.Life;
        MinLife = lso.MinLife;
        MaxLife = lso.MaxLife;
        ConstantLife = lso.ConstantLife;

        Speed = lso.Speed;
        MinSpeed = lso.MinSpeed;
        MaxSpeed = lso.MaxSpeed;
        ConstantSpeed = lso.ConstantSpeed;

        Size = lso.Size;
        MinSize = lso.MinSize;
        MaxSize = lso.MaxSize;
        ConstantSize = lso.ConstantSize;
        ParticleSize = lso.ParticleSize;
        SizeOverLifeTime = lso.SizeOverLifeTime;

        Intensity = lso.Intensity;
        Gravity = lso.Gravity;
        Angle = lso.Angle;
        Radius = lso.Radius;

        Color = lso.Color;
        ColorGradient = lso.ColorGradient;
        ColorOverLifeTime = lso.ColorOverLifeTime;
        EnableCollision = lso.EnableCollision;

        emmision = lso.emmision;
        shape = lso.shape;
        sizeOverLifeTime = lso.sizeOverLifeTime;
        colorOverLifeTime = lso.colorOverLifeTime;
        collision = lso.collision;
        CollidesWith = lso.CollidesWith;

        Dampen = lso.Dampen;
        LifetimeLoss = lso.LifetimeLoss;
        Bounce = lso.Bounce;



        GenerateLaser =  lso.GenerateLaser;
        GenerateLife = lso.GenerateLife;
        GenerateSpeed = lso.GenerateSpeed;
        GenerateSize = lso.GenerateSize;
        GenerateColor = lso.GenerateColor;
        GenerateAngle = lso.GenerateAngle;
        GenerateRadius = lso.GenerateRadius;
        GenerateIntensity = lso.GenerateIntensity;
        GenerationScale = lso.GenerationScale;
        LifeSlider = lso.LifeSlider;
        SpeedSlider = lso.SpeedSlider;
        SizeSlider = lso.SizeSlider;
        AngleSlider = lso.AngleSlider;
        RadiusSlider = lso.RadiusSlider;
        IntensitySlider = lso.IntensitySlider;


        LifeOptionIndex = lso.LifeOptionIndex;
        SizeOptionIndex = lso.SizeOptionIndex;
        SpeedOptionIndex = lso.SpeedOptionIndex;
#endif

    }
    void AssignValuesToScriptableObject()
    {

        if (lso != null)
        {

            lso.position = transform.position;
            lso.rotation = transform.rotation;

            lso.laserMaterial = laserMaterial;
            lso.Life = Life;
            lso.MinLife = MinLife;
            lso.MaxLife = MaxLife;
            lso.ConstantLife = ConstantLife;

            lso.Speed = Speed;
            lso.MinSpeed = MinSpeed;
            lso.MaxSpeed = MaxSpeed;
            lso.ConstantSpeed = ConstantSpeed;

            lso.Size = Size;
            lso.MinSize = MinSize;
            lso.MaxSize = MaxSize;
            lso.ConstantSize = ConstantSize;
            lso.ParticleSize = ParticleSize;
            lso.SizeOverLifeTime = SizeOverLifeTime;

            lso.Intensity = Intensity;
            lso.Gravity = Gravity;
            lso.Angle = Angle;
            lso.Radius = Radius;

            lso.Color = Color;
            lso.ColorGradient = ColorGradient;
            lso.ColorOverLifeTime = ColorOverLifeTime;
            lso.EnableCollision = EnableCollision;

            lso.emmision = emmision;
            lso.shape = shape;
            lso.sizeOverLifeTime = sizeOverLifeTime;
            lso.colorOverLifeTime = colorOverLifeTime;
            lso.collision = collision;
            lso.CollidesWith = CollidesWith;

            lso.Dampen = Dampen;
            lso.LifetimeLoss = LifetimeLoss;
            lso.Bounce = Bounce;

            lso.OnLaserParticleCollision = OnLaserParticleCollision;


            lso.GenerateLaser =  GenerateLaser;
            lso.GenerateLife =  GenerateLife;
            lso.GenerateSpeed =  GenerateSpeed;
            lso.GenerateSize =  GenerateSize;
            lso.GenerateColor =  GenerateColor;
            lso.GenerateAngle =  GenerateAngle;
            lso.GenerateRadius =  GenerateRadius;
            lso.GenerateIntensity =  GenerateIntensity;
            lso.GenerationScale = GenerationScale;
            lso.LifeSlider =  LifeSlider;
            lso.SpeedSlider =  SpeedSlider;
            lso.SizeSlider =  SizeSlider;
            lso.AngleSlider =  AngleSlider;
            lso.RadiusSlider =  RadiusSlider;
            lso.IntensitySlider =  IntensitySlider;
            lso.LifeOptionIndex = LifeOptionIndex;
            lso.SpeedOptionIndex = SpeedOptionIndex;
            lso.SizeOptionIndex = SizeOptionIndex;


        }
    }





    /// <summary>
    /// Method used for setting lower bounds for the variables
    /// </summary>
    void SetBounds()
    {
        Life = (Life < 0) ? 0 : Life;
        Size = (Size < 0) ? 0 : Size;
        Speed = (Speed < 0) ? 0 : Speed;
        Angle = (Angle < 0) ? 0 : Angle;
        Radius = (Radius < 0) ? 0 : Radius;

        MaxSize = (MaxSize < 0) ? 0 : MaxSize;

        MinSpeed = (MinSpeed < 0) ? 0 : MinSpeed;
        MaxSpeed = (MaxSpeed < 0) ? 0 : MaxSpeed;


        MinLife = (MinLife < 0) ? 0 : MinLife;
        MaxLife = (MaxLife < 0) ? 0 : MaxLife;

        Intensity = (Intensity < 0) ? 0 : Intensity;
        GenerationScale = (GenerationScale < 0) ? .1f : GenerationScale;
    }


    void LaserParicleCollison(string method)
    {
        OnLaserParticleCollision.Invoke(method);
    }

    public void Generate()
    {
        generator.Generate();
    }


    void OnParticleCollision(GameObject other)
    {


        if (OnLaserParticleCollision.GetPersistentEventCount() > 0)
        {
            // Debug.Log(OnLaserParticleCollision.GetPersistentTarget(0));
            LaserParicleCollison(OnLaserParticleCollision.GetPersistentMethodName(0));
        }
    }




}

[Serializable]
public class CollisionEnter : UnityEvent<string>
{


}


