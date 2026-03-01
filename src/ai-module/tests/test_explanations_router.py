from fastapi.testclient import TestClient
from app.main import app

client = TestClient(app)


def test_explanations_endpoint_exists():
    response = client.post("/api/explanations", json={
        "topic": "Test",
        "subject": 1,
        "difficulty_level": 1,
        "student_age": 14
    })
    assert response.status_code != 404


def test_explanations_invalid_subject_returns_422():
    response = client.post("/api/explanations", json={
        "topic": "Test",
        "subject": 99,
        "difficulty_level": 1,
        "student_age": 14
    })
    assert response.status_code == 422


def test_hint_endpoint_exists():
    response = client.post("/api/explanations/hint", json={
        "question": "Test?",
        "subject": 1
    })
    assert response.status_code != 404
