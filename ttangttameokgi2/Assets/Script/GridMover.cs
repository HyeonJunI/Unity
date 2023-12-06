using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // TextMeshPro 네임스페이스 추가

public class GridMover : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float gridSize = 5f; // 그리드 크기 설정
    public TextMeshProUGUI scoreText; // TMP 텍스트 UI 참조
    public TextMeshProUGUI timeText; // TMP 텍스트 UI 참조 제한 시간을 표시하기 위한 변수 추가
    public TextMeshProUGUI messageText; // TMP 텍스트 UI 참조 게임 메시지를 표시하기 위한 변수 추가

    private int score = 0; // 점수
    private float timeLeft = 30f; // 남은 시간 설정
    private bool isGameActive = true; // 게임 상태를 나타내는 변수 추가

    private Vector3 originalPosition, targetPosition;
    private bool isMoving = false;

    void Start()
    {
        originalPosition = transform.position;
        ChangeBlockColor(); // 게임 시작 시 첫 위치의 타일 색상 변경
        UpdateScoreText(); // 점수 텍스트 초기화
        UpdateTimeText(); // 시간 텍스트 초기화
        UpdateMessageText("Take the land and win the victory");
    }

    void Update()
    {
        if (isGameActive)
        {
            if (timeLeft > 0) // 시간이 남아있을 때만 게임 진행
            {
                if (!isMoving)
                {
                    float xInput = Input.GetAxisRaw("Horizontal");
                    float zInput = Input.GetAxisRaw("Vertical");

                    if (xInput != 0 || zInput != 0)
                    {
                        StartCoroutine(MovePlayer(xInput, zInput));
                    }
                }

                // 시간 감소
                timeLeft -= Time.deltaTime;
                UpdateTimeText(); // 시간 텍스트 업데이트

                if (timeLeft <= 0)
                {
                    // 시간 종료 처리
                    EndGame(false); // 패배
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
        ChangeBlockColor();
        isMoving = false;
    }

    private void ChangeBlockColor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // 색상 변경 검사
                    if (renderer.material.color != Color.red)
                    {
                        renderer.material.color = Color.red; // 색상 변경
                        IncreaseScore(); // 점수 증가
                    }
                }
            }
        }
    }

    private void IncreaseScore()
    {
        score++; // 점수 증가
        UpdateScoreText(); // 점수 텍스트 업데이트
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score; // 점수 표시
    }

    private void UpdateTimeText()
    {
        timeText.text = "Time: " + Mathf.Ceil(timeLeft).ToString(); // 시간 표시 (올림해서 정수로 표시)
    }

    private void UpdateMessageText(string message)
    {
        messageText.text = message; // 게임 메시지 표시
    }

    private void EndGame(bool win)
    {
        isGameActive = false;
        if (win)
        {
            UpdateMessageText("Congratulations. You win!!");
        }
        else
        {
            UpdateMessageText("Next time...");
        }
    }
}
