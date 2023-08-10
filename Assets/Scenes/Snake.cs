using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments = new();
    public Transform segmentPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ResetState();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 expected_direction = _direction;
        if (Input.GetKeyDown(KeyCode.W))
        {
            expected_direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            expected_direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            expected_direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            expected_direction = Vector2.right;
        }

        // can't make 180 turn when snake size > 1
        bool enableDirectionChange = true;
        if (_segments.Count > 1 && expected_direction == -_direction)
        {
            enableDirectionChange = false;
        }

        if (enableDirectionChange)
        {
            _direction = expected_direction;
        }
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        this.transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
        }
        else if (other.CompareTag("Obstacle"))
        {
            ResetState();
        }
    }
}
