from fastapi.testclient import TestClient
from app.main import app

client = TestClient(app)


def test_problems_endpoint_exists():
    response = client.post("/api/problems", json={
        "topic": "Test",
        "subject": 1,
        "difficulty_level": 1,
        "student_age": 14,
        "count": 1
    })
    assert response.status_code != 404


def test_problems_invalid_topic_returns_422():
    response = client.post("/api/problems", json={
        "topic": "",
        "subject": 1,
        "difficulty_level": 1,
        "student_age": 14,
        "count": 1
    })
    assert response.status_code == 422
