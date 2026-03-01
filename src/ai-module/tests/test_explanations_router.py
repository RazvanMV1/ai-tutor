import pytest
from unittest.mock import AsyncMock, patch
from fastapi.testclient import TestClient
from app.main import app
from app.schemas.ai_schemas import (
    ExplanationResponse,
    HintResponse,
    SubjectType,
    DifficultyLevel
)

client = TestClient(app)


def test_create_explanation_mock():
    mock_response = ExplanationResponse(
        topic="Ecuații de gradul 2",
        explanation="O ecuație de gradul 2 este...",
        examples=["x² + 2x + 1 = 0", "x² - 4 = 0"],
        key_points=["Discriminantul", "Rădăcinile"],
        subject=SubjectType.MATHEMATICS,
        difficulty_level=DifficultyLevel.BEGINNER
    )

    with patch("app.routers.explanations.get_explanation",
               new_callable=AsyncMock) as mock_get:
        mock_get.return_value = mock_response

        response = client.post("/api/explanations", json={
            "topic": "Ecuații de gradul 2",
            "subject": 1,
            "difficulty_level": 1,
            "student_age": 14
        })

        assert response.status_code == 200
        data = response.json()
        assert data["topic"] == "Ecuații de gradul 2"
        assert "explanation" in data
        assert "examples" in data
        assert "key_points" in data


def test_create_hint_mock():
    mock_response = HintResponse(
        question="Ce este x în ecuația x² = 4?",
        hint="Încearcă să calculezi radical din 4",
        subject=SubjectType.MATHEMATICS
    )

    with patch("app.routers.explanations.get_hint",
               new_callable=AsyncMock) as mock_get:
        mock_get.return_value = mock_response

        response = client.post("/api/explanations/hint", json={
            "question": "Ce este x în ecuația x² = 4?",
            "subject": 1
        })

        assert response.status_code == 200
        data = response.json()
        assert "hint" in data


def test_create_explanation_invalid_subject():
    response = client.post("/api/explanations", json={
        "topic": "Test",
        "subject": 99,
        "difficulty_level": 1,
        "student_age": 14
    })
    assert response.status_code == 422
