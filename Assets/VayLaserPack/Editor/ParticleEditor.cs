using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParticleManager))]
public class ParticleEditor : Editor
{
    private SerializedObject m_object;

    private SerializedProperty m_Life, m_Size, m_Color, m_showColor, m_intensity, m_gravity, m_speed, m_angle, m_radius;

    private SerializedProperty m_Size_min, m_Size_max, m_constantSize, m_Speed_min, m_Speed_max, m_constantSpeed, m_Life_min, m_Life_max, m_constantLife, m_laserMaterial;

    private SerializedProperty m_animationCurve, m_colorGradient, m_sizeOverLifeTime, m_colorOverLifeTime, m_enableCollision;

    private SerializedProperty m_shouldPersist, m_didApplicationQuit;

    private SerializedProperty m_collision_dampen, m_collision_lifeTimeLoss, m_collision_bounce, m_collision_layerMask, m_collisionEnter;

    private SerializedProperty m_generate_Laser, m_generate_life, m_generate_speed, m_generate_size, m_generate_color, m_generate_angle, m_generate_radius, m_generate_intensity;

    private SerializedProperty m_lifeSlider, m_speedSlider, m_sizeSlider,m_angleSlider,m_radiusSlider,m_intensitySlider, m_generationScale;

    private SerializedProperty m_LifeOptionIndex, m_SizeOptionIndex, m_SpeedOptionIndex; 

    private int lifeOptionIndex;
    private string[] lifeOptions = { "Constant", "Random" };


    private int sizeOptionIndex;
    private string[] sizeOptions = { "Constant", "Random", "SizeOverLifeTime" };

    private int speedOptionIndex;
    private string[] speedOptions = { "Constant", "Random" };




    void Awake()
    {
        lifeOptionIndex = speedOptionIndex = sizeOptionIndex = 0;
    }

    public void OnEnable()
    {
        m_object = new SerializedObject(target);

        lifeOptionIndex = EditorPrefs.GetInt("LifeOption");
        speedOptionIndex = EditorPrefs.GetInt("SpeedOption");
        sizeOptionIndex = EditorPrefs.GetInt("SizeOption");


        m_Life = m_object.FindProperty("Life");
        m_Size = m_object.FindProperty("Size");
        m_intensity = m_object.FindProperty("Intensity");
        m_speed = m_object.FindProperty("Speed");
        m_gravity = m_object.FindProperty("Gravity");


        m_Color = m_object.FindProperty("Color");
        m_showColor = m_object.FindProperty("showColor");
        m_angle = m_object.FindProperty("Angle");
        m_radius = m_object.FindProperty("Radius");

        m_animationCurve = m_object.FindProperty("ParticleSize");
        m_sizeOverLifeTime = m_object.FindProperty("SizeOverLifeTime");
        m_colorGradient = m_object.FindProperty("ColorGradient");

        m_Size_min = m_object.FindProperty("MinSize");
        m_Size_max = m_object.FindProperty("MaxSize");


        m_Speed_min = m_object.FindProperty("MinSpeed");
        m_Speed_max = m_object.FindProperty("MaxSpeed");

        m_Life_min = m_object.FindProperty("MinLife");
        m_Life_max = m_object.FindProperty("MaxLife");



        m_constantSize = m_object.FindProperty("ConstantSize");
        m_constantSpeed = m_object.FindProperty("ConstantSpeed");
        m_constantLife = m_object.FindProperty("ConstantLife");
        m_colorOverLifeTime = m_object.FindProperty("ColorOverLifeTime");



        m_didApplicationQuit = m_object.FindProperty("didApplicationQuit");

        m_laserMaterial = m_object.FindProperty("laserMaterial");

        m_enableCollision = m_object.FindProperty("EnableCollision");

        m_collision_dampen = m_object.FindProperty("Dampen");
        m_collision_lifeTimeLoss = m_object.FindProperty("LifetimeLoss");
        m_collision_bounce = m_object.FindProperty("Bounce");
        m_collision_layerMask = m_object.FindProperty("CollidesWith");

        m_collisionEnter = m_object.FindProperty("OnLaserParticleCollision");

        m_generate_Laser = m_object.FindProperty("GenerateLaser");
        m_generate_life = m_object.FindProperty("GenerateLife");
        m_generate_speed = m_object.FindProperty("GenerateSpeed");
        m_generate_size = m_object.FindProperty("GenerateSize");
        m_generate_color = m_object.FindProperty("GenerateColor");
        m_generate_angle = m_object.FindProperty("GenerateAngle");
        m_generate_radius = m_object.FindProperty("GenerateRadius");
        m_generate_intensity = m_object.FindProperty("GenerateIntensity");


        m_lifeSlider = m_object.FindProperty("LifeSlider");
        m_speedSlider = m_object.FindProperty("SpeedSlider");
        m_sizeSlider = m_object.FindProperty("SizeSlider");
        m_angleSlider = m_object.FindProperty("AngleSlider");
        m_radiusSlider = m_object.FindProperty("RadiusSlider");
        m_intensitySlider = m_object.FindProperty("IntensitySlider");
        m_generationScale = m_object.FindProperty("GenerationScale");

        m_LifeOptionIndex = m_object.FindProperty("LifeOptionIndex"); 
        m_SizeOptionIndex = m_object.FindProperty("SizeOptionIndex"); 
        m_SpeedOptionIndex = m_object.FindProperty("SpeedOptionIndex"); 

        ReassignIndexValues();

       
    }


    void ReassignIndexValues()
    {
       
        lifeOptionIndex = m_LifeOptionIndex.intValue; 
        sizeOptionIndex = m_SizeOptionIndex.intValue; 
        speedOptionIndex = m_SpeedOptionIndex.intValue; 

    }


    void OnDisable()
    {
        EditorPrefs.SetInt("LifeOption", lifeOptionIndex);
        EditorPrefs.SetInt("SizeOption", sizeOptionIndex);
        EditorPrefs.SetInt("SpeedOption", speedOptionIndex);
    }




    /// <summary>
    /// GUI for Particle Manager
    /// </summary>

    bool valueChanging; 
    public override void OnInspectorGUI()
    {
       
     
         m_object.Update();

        m_LifeOptionIndex.intValue = lifeOptionIndex; 
        m_SizeOptionIndex.intValue = sizeOptionIndex; 
        m_SpeedOptionIndex.intValue = speedOptionIndex; 
      
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(m_generate_Laser);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.EndHorizontal(); 

        if (m_generate_Laser.boolValue == true)
        {

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_generationScale); 
            // EditorGUILayout.BeginVertical(); 


            EditorGUILayout.BeginHorizontal(); 
            EditorGUILayout.PropertyField(m_generate_life);
            EditorGUILayout.LabelField("Life Intensity", GUILayout.Width(150)); 
            float lifeValueChange = m_lifeSlider.floatValue; 
            m_lifeSlider.floatValue = GUILayout.HorizontalSlider(m_lifeSlider.floatValue, 0.0f, m_generationScale.floatValue, GUILayout.Width(250));            
            if(lifeValueChange != m_lifeSlider.floatValue)
            {
               m_generate_life.boolValue = true;                 
            }
              
              

            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_generate_speed);
            EditorGUILayout.LabelField("Speed Intensity", GUILayout.Width(150));
            float SpeedValueChange = m_speedSlider.floatValue; 
            m_speedSlider.floatValue = GUILayout.HorizontalSlider(m_speedSlider.floatValue, 0.0f, m_generationScale.floatValue, GUILayout.Width(250));
            if(SpeedValueChange != m_speedSlider.floatValue){
                m_generate_speed.boolValue = true; 
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_generate_size);
            EditorGUILayout.LabelField("Size Intensity", GUILayout.Width(150));
            float SizeValueChange = m_sizeSlider.floatValue; 
            m_sizeSlider.floatValue = GUILayout.HorizontalSlider(m_sizeSlider.floatValue, 0.0f, m_generationScale.floatValue, GUILayout.Width(250));
            if(SizeValueChange != m_sizeSlider.floatValue){
                m_generate_size.boolValue = true; 
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(m_generate_color);


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_generate_angle);
            EditorGUILayout.LabelField("Angle Intensity", GUILayout.Width(150));
            float AngleSliderValue= m_angleSlider.floatValue; 
            m_angleSlider.floatValue = GUILayout.HorizontalSlider(m_angleSlider.floatValue, 0.0f, m_generationScale.floatValue, GUILayout.Width(250));
            if(AngleSliderValue != m_angleSlider.floatValue){
             m_generate_angle.boolValue = true; 
            }
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_generate_radius);
            EditorGUILayout.LabelField("Radius Intensity", GUILayout.Width(150));
            float RadiusSliderValue= m_radiusSlider.floatValue; 
            m_radiusSlider.floatValue = GUILayout.HorizontalSlider(m_radiusSlider.floatValue, 0.0f, m_generationScale.floatValue, GUILayout.Width(250));
            if(RadiusSliderValue != m_radiusSlider.floatValue){
              m_generate_radius.boolValue = true; 
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
          
            EditorGUILayout.PropertyField(m_generate_intensity);
            EditorGUILayout.LabelField("Intensity Intensity", GUILayout.Width(150));
            float IntensitySliderValue= m_intensitySlider.floatValue; 
            m_intensitySlider.floatValue = GUILayout.HorizontalSlider(m_intensitySlider.floatValue, 0.0f, m_generationScale.floatValue, GUILayout.Width(250));
            if(IntensitySliderValue != m_intensitySlider.floatValue){
              m_generate_intensity.boolValue = true; 
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(); 
            EditorGUILayout.BeginHorizontal();
          
            if (GUILayout.Button("Select All"))
            {
                m_generate_life.boolValue = true; 
                m_generate_speed.boolValue = true; 
                m_generate_size.boolValue = true; 
                m_generate_color.boolValue = true; 
                m_generate_angle.boolValue = true; 
                m_generate_radius.boolValue = true; 
                m_generate_intensity.boolValue = true; 
                   
            }


            if(GUILayout.Button("Deselect All")){
                m_generate_life.boolValue = false; 
                m_generate_speed.boolValue = false; 
                m_generate_size.boolValue = false; 
                m_generate_color.boolValue = false; 
                m_generate_angle.boolValue = false; 
                m_generate_radius.boolValue = false; 
                m_generate_intensity.boolValue = false; 
                   
            }

           EditorGUILayout.EndHorizontal(); 


            EditorGUILayout.Space(); 
            if (GUILayout.Button("Generate Laser"))
            {

                ParticleManager p = (ParticleManager)target;
                p.Generate();
            }


            EditorGUI.indentLevel--; 

        }




      
        EditorGUILayout.Space();
        GUILayout.Label("Laser Material", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(m_laserMaterial);
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();


        GUILayout.Label("Life Properties", EditorStyles.boldLabel);

        lifeOptionIndex = EditorGUILayout.Popup("Life Options", lifeOptionIndex, lifeOptions);

        if (!lifeOptions[lifeOptionIndex].Equals("Random"))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_Life);
            m_constantLife.boolValue = true;
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_Life_min);
            EditorGUILayout.PropertyField(m_Life_max);
            m_constantLife.boolValue = false;
            EditorGUI.indentLevel--;

        }

        EditorGUILayout.Space();
        GUILayout.Label("Speed Properties", EditorStyles.boldLabel);
        speedOptionIndex = EditorGUILayout.Popup("Speed Options", speedOptionIndex, speedOptions);

        if (!speedOptions[speedOptionIndex].Equals("Random"))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_speed);
            m_constantSpeed.boolValue = true;
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_Speed_min);
            EditorGUILayout.PropertyField(m_Speed_max);
            m_constantSpeed.boolValue = false;
            EditorGUI.indentLevel--;

        }

        EditorGUILayout.Space();
        GUILayout.Label("Size Properties", EditorStyles.boldLabel);
        sizeOptionIndex = EditorGUILayout.Popup("Size Options", sizeOptionIndex, sizeOptions);


        if (sizeOptions[sizeOptionIndex].Equals("Constant"))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_Size);
            m_constantSize.boolValue = true;
            m_sizeOverLifeTime.boolValue = false;
            EditorGUI.indentLevel--;
        }
        else if (sizeOptions[sizeOptionIndex].Equals("Random"))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_Size_min);
            EditorGUILayout.PropertyField(m_Size_max);
            m_constantSize.boolValue = false;
            m_sizeOverLifeTime.boolValue = false;
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_animationCurve);
            m_sizeOverLifeTime.boolValue = true;
            m_constantSize.boolValue = false;
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.Space();



        GUILayout.Label("Color Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_colorOverLifeTime);

        if (m_colorOverLifeTime.boolValue == true)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_colorGradient);
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_Color);
            EditorGUI.indentLevel--;
        }



        EditorGUILayout.Space();
        GUILayout.Label("Collision Properties", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(m_enableCollision);
        if (m_enableCollision.boolValue == true)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_collision_dampen);
            EditorGUILayout.PropertyField(m_collision_lifeTimeLoss);
            EditorGUILayout.PropertyField(m_collision_bounce);
            EditorGUILayout.PropertyField(m_collision_layerMask);
            EditorGUILayout.Space(); 
            EditorGUILayout.PropertyField(m_collisionEnter);

            EditorGUI.indentLevel--;
        }


        EditorGUILayout.Space();
        GUILayout.Label("Other Properties", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(m_gravity);
        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(m_angle);
        EditorGUILayout.PropertyField(m_radius);
        GUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(m_intensity);
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Laser Prefab"))
        {
            if (AssetDatabase.IsValidFolder("Assets/VayLaserPack/Resources/LaserPrefabs") == false)
            {
                AssetDatabase.CreateFolder("Assets/VayLaserPack/Resources", "LaserPrefabs");
            }

            AssetDatabase.DeleteAsset("Assets/VayLaserPack/Resources/LaserPrefabs/" + m_object.targetObject.name + ".prefab");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            PrefabUtility.CreatePrefab("Assets/VayLaserPack/Resources/LaserPrefabs/" + m_object.targetObject.name + ".prefab", Selection.activeGameObject);
            AssetDatabase.SaveAssets();
        }





        m_object.ApplyModifiedProperties();
    }
}
