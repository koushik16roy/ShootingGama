using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 5;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives =3 ;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoosterActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    //[SerializeField]
    //private GameObject _rightEngine,_leftEngine;
    [SerializeField]
    private int _score,_bestscore;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    void Start()
    {
        transform.position=new Vector3(0,0,0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }
        if(_uiManager == null)
        {
            Debug.LogError("The UI manager is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
         
    }
    public void Update()
    {
        CalculateMovement();
            FireLaser();
        
    }
    public void CalculateMovement()
    {
        float horizontalInput =  Input.GetAxis("Horizontal"); // CrossPlatformInputManager.GetAxis("Horizontal");    
        float verticalInput =   Input.GetAxis("Vertical");  // CrossPlatformInputManager.GetAxis("Vertical");  
        //transform.Translate(Vector3.right* horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        Vector3 direction = new Vector3(horizontalInput,verticalInput,0);
        
        transform.Translate(direction * _speed * Time.deltaTime);

        if(transform.position.y >=0 )
        {
            transform.position = new Vector3(transform.position.x,0,0);
        }
        else if(transform.position.y <= -4.0f)
        {
            transform.position = new Vector3(transform.position.x,-4.0f,0); 
        }

        //transform.position=new Vector3(transform.position.x,Math.Clamp(transform.position.y,-4.0f),0);

        if(transform.position.x <= -8.0f)
        {
            transform.position = new Vector3(-8.0f,transform.position.y,0);
        }
        else if(transform.position.x >= 8.0f)
        {
            transform.position = new Vector3(8.0f,transform.position.y,0);
        }
    }
    void FireLaser()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;

            if(_isTripleShotActive == true)
            {
               Instantiate(_tripleshotPrefab,transform.position,Quaternion.identity);
            }
            else
            {
               Instantiate(_laserPrefab,transform.position + new Vector3(0,1.05f,0),Quaternion.identity);
            }
        _audioSource.Play();
        }
    }
    public void Damage()
    {
        if(_isShieldActive == true)
        {
            _isShieldActive=false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        /*if(_lives == 2)
         {
             _leftEngine.SetActive(true);
         }
         else if(_lives ==1)
         {
             _rightEngine.SetActive(true);
         } */
        _uiManager.UpdateLives(_lives);
        if(_lives < 1 )
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.CheckForBestScore(_bestscore);
            Destroy(this.gameObject);
        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());  
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedBoosterActive()
    {
        _isSpeedBoosterActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoosterPowerDownRoutine());
    }
    IEnumerator SpeedBoosterPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
       _isSpeedBoosterActive = false;
       _speed /= _speedMultiplier; 
    }
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true); 
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
        _uiManager.CheckForBestScore(_bestscore);
    }
}
