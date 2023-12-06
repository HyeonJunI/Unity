using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover3 : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float gridSize = 5f; // 그리드 크기 설정
    public float directionChangeInterval = 2f; // 방향 변경 간격 설정
    public Vector3 mapMin; // 이동 가능한 최소 좌표
    public Vector3 mapMax; // 이동 가능한 최대 좌표

    private Vector3 originalPosition, targetPosition;
    private bool isMoving = false;

    void Start()
    {
        originalPosition = transform.position;
        ChangeBlockColor(); // 게임 시작 시 첫 위치의 타일 색상 변경
        StartCoroutine(NewDirection());
    }

    IEnumerator NewDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(directionChangeInterval);

            if (!isMoving)
            {
                // 무작위 방향 생성
                float xInput = Random.Range(-1, 2); // -1, 0, 또는 1
                float zInput = Random.Range(-1, 2); // -1, 0, 또는 1

                Vector3 potentialDirection = new Vector3(xInput, 0, zInput).normalized;
                Vector3 potentialPosition = originalPosition + potentialDirection * gridSize;

                // 예상 위치가 이동 범위 내에 있는지 확인
                if (potentialPosition.x >= mapMin.x && potentialPosition.x <= mapMax.x &&
                    potentialPosition.z >= mapMin.z && potentialPosition.z <= mapMax.z)
                {
                    StartCoroutine(MovePlayer(xInput, zInput));
                }
            }
        }
    }

    IEnumerator MovePlayer(float xInput, float zInput)
    {
        isMoving = true;
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(xInput, 0, zInput) * gridSize;

        float elapsedTime = 0;

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        ChangeBlockColor(); // 여기에서 함수 호출
        isMoving = false;
    }

    private void ChangeBlockColor()
    {
        RaycastHit hit;
        // 플레이어의 위치에서 아래로 Raycast를 발사하여 바닥 타일 감지
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f))
        {
            // 타일의 태그 확인
            if (hit.collider.CompareTag("Ground"))
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.blue; // 색상 변경
                }
            }
        }
    }

}
