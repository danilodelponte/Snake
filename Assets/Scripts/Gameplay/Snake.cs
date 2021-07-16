using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Snake : MonoBehaviour
{
    public Player Player { get; set; }
    public bool IsPlayer { get => Player != null; }
    public bool isAI { get => GetComponent<AIControl>() != null; }

    public Color Color { get => color; set => SetColor(value); }
    private Color color;

    public SnakeSegment Head { get; set; }

    public SnakeSegment AddHead() {
        if(Head != null) return null;

        Head = SnakeSegmentRepository.Build(transform.position, transform.rotation, this);
        Head.Color = color;
        return Head;
    }

    public SnakeSegment AddSegment(Vector3 direction) {
        Vector3 position = Head.transform.position + direction.normalized;
        Quaternion rotation = GetRotation(direction);

        SnakeSegment newHead = SnakeSegmentRepository.Build(position, rotation, this);
        newHead.NextSegment = Head;
        newHead.Color = Color;
        Head = newHead;
	    return Head;
    }

    private void SetColor(Color color) {
        this.color = color;
        Head.Color = color;
    }

    public IEnumerable<SnakeSegment> Segments() { 
        SnakeSegment segment = Head;
        while(segment != null) {
            yield return segment;
            segment = segment.NextSegment;
        }
    }

    private Quaternion GetRotation(Vector3 direction) {
        direction.Normalize();
        Quaternion rotation = Quaternion.identity;
        if(direction.x == -1) rotation = Quaternion.Euler(0,0,90);
        else if(direction.y == -1) rotation = Quaternion.Euler(0,0,180);
        else if(direction.x == 1) rotation = Quaternion.Euler(0,0,270);
        return rotation;
    }
}
