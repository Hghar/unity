using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class DisappearingText : MonoBehaviour
    {
        [SerializeField] [Min(0.001f)] private float _lifeTime;
        [SerializeField] private float _jumpHeight;

        private TMP_Text _text;
        private float _movingSpeed;
        private float _colorSpeed;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _movingSpeed = _jumpHeight / _lifeTime;
        }

        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            transform.Translate(Vector2.up * _movingSpeed * Time.deltaTime);
            float newAlpha = _text.color.a - (_colorSpeed * Time.deltaTime);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, newAlpha);
        }

        public void Init(string text, Color color)
        {
            _text.text = text;
            _text.color = color;
            _colorSpeed = _text.color.a / _lifeTime;
        }
    }
}