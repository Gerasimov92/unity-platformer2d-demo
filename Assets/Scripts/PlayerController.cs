using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Collider2D groundCollider2D;
    [SerializeField] private Collider2D wallCollider2D;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public LayerMask ground;
    public float speed;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        var onTheGround = groundCollider2D.IsTouchingLayers(ground);

        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0.0f)
        {
            if (onTheGround)
            {
                _rigidbody2D.velocity = new Vector2(-speed, _rigidbody2D.velocity.y);
                _animator.SetBool("running", true);
            }
            else
            {
                _animator.SetBool("running", false);
            }

            _spriteRenderer.flipX = true;
        }
        else if (hDirection > 0.0f)
        {
            if (onTheGround)
            {
                _rigidbody2D.velocity = new Vector2(speed, _rigidbody2D.velocity.y);
                _animator.SetBool("running", true);
            }
            else
            {
                _animator.SetBool("running", false);
            }

            _spriteRenderer.flipX = false;
        }
        else
        {
            _animator.SetBool("running", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onTheGround)
            {
                _animator.SetBool("running", false);
                _animator.SetTrigger("jump");
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, speed);
            }
            else if (wallCollider2D.IsTouchingLayers(ground))
            {
                _animator.SetTrigger("jump");
                var flipX = _spriteRenderer.flipX;
                _rigidbody2D.velocity = new Vector2(flipX ? speed : -speed, speed);
                _spriteRenderer.flipX = !flipX;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            int layer = LayerMask.NameToLayer("Player");
            int mask = Physics2D.GetLayerCollisionMask(layer);
            mask |= LayerMask.GetMask("Crate");
            Physics2D.SetLayerCollisionMask(layer, mask);

            Destroy(other.gameObject);
        }
    }
}
