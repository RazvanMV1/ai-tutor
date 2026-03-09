import os
import json
import google.generativeai as genai
from app.schemas.ai_schemas import (
    ExplanationRequest, ExplanationResponse,
    HintRequest, HintResponse,
    ProblemRequest, ProblemResponse,
    SubjectType, DifficultyLevel
)

GEMINI_API_KEY = os.getenv("GEMINI_API_KEY", "")

def get_gemini_client():
    if not GEMINI_API_KEY:
        return None
    genai.configure(api_key=GEMINI_API_KEY)
    return genai.GenerativeModel("gemini-2.0-flash")

SUBJECT_NAMES = {
    SubjectType.MATHEMATICS: "Matematică",
    SubjectType.ROMANIAN: "Limba Română",
    SubjectType.INFORMATICS: "Informatică"
}

DIFFICULTY_NAMES = {
    DifficultyLevel.BEGINNER: "începător",
    DifficultyLevel.INTERMEDIATE: "intermediar",
    DifficultyLevel.ADVANCED: "avansat"
}


async def get_explanation(request: ExplanationRequest) -> ExplanationResponse:
    model = get_gemini_client()

    if not model:
        return ExplanationResponse(
            topic=request.topic,
            explanation=f"[DEMO] Explicație pentru '{request.topic}': Acesta este un răspuns demonstrativ.",
            examples=[f"Exemplu 1 pentru {request.topic}", f"Exemplu 2 pentru {request.topic}"],
            key_points=["Punct cheie 1", "Punct cheie 2", "Punct cheie 3"],
            subject=request.subject,
            difficulty_level=request.difficulty_level
        )

    prompt = f"""Ești un profesor expert în {SUBJECT_NAMES.get(request.subject, 'materie')}.
Explică '{request.topic}' unui elev de {request.student_age} ani, nivel {DIFFICULTY_NAMES.get(request.difficulty_level, 'începător')}.
Răspunde DOAR în format JSON valid, fără markdown, fără ```json, doar JSON pur:
{{
  "explanation": "explicație detaliată aici",
  "examples": ["exemplu 1", "exemplu 2", "exemplu 3"],
  "key_points": ["punct cheie 1", "punct cheie 2", "punct cheie 3"]
}}"""

    response = model.generate_content(prompt)
    text = response.text.strip()

    # Curăță markdown dacă există
    if text.startswith("```"):
        text = text.split("```")[1]
        if text.startswith("json"):
            text = text[4:]
    text = text.strip()

    data = json.loads(text)

    return ExplanationResponse(
        topic=request.topic,
        explanation=data.get("explanation", ""),
        examples=data.get("examples", []),
        key_points=data.get("key_points", []),
        subject=request.subject,
        difficulty_level=request.difficulty_level
    )


async def get_hint(request: HintRequest) -> HintResponse:
    model = get_gemini_client()

    if not model:
        return HintResponse(
            question=request.question,
            hint="[DEMO] Indiciu demonstrativ. Adaugă GEMINI_API_KEY pentru indicii reale.",
            subject=request.subject
        )

    prompt = f"""Oferă un indiciu util (fără să dai răspunsul complet) pentru: '{request.question}'.
Răspunde DOAR în format JSON valid, fără markdown:
{{
  "hint": "indiciul aici"
}}"""

    response = model.generate_content(prompt)
    text = response.text.strip()

    if text.startswith("```"):
        text = text.split("```")[1]
        if text.startswith("json"):
            text = text[4:]
    text = text.strip()

    data = json.loads(text)

    return HintResponse(
        question=request.question,
        hint=data.get("hint", ""),
        subject=request.subject
    )


async def generate_problems(request: ProblemRequest) -> ProblemResponse:
    model = get_gemini_client()

    if not model:
        return ProblemResponse(
            topic=request.topic,
            problems=[f"[DEMO] Problemă {i+1} despre {request.topic}" for i in range(request.count)],
            hints=[f"Indiciu {i+1}" for i in range(request.count)],
            solutions=[f"Soluție demonstrativă {i+1}" for i in range(request.count)],
            subject=request.subject,
            difficulty_level=request.difficulty_level
        )

    prompt = f"""Generează {request.count} probleme despre '{request.topic}' pentru un elev de {request.student_age} ani, nivel {DIFFICULTY_NAMES.get(request.difficulty_level, 'începător')}.
Răspunde DOAR în format JSON valid, fără markdown:
{{
  "problems": ["problemă 1", "problemă 2"],
  "hints": ["indiciu 1", "indiciu 2"],
  "solutions": ["soluție 1", "soluție 2"]
}}"""

    response = model.generate_content(prompt)
    text = response.text.strip()

    if text.startswith("```"):
        text = text.split("```")[1]
        if text.startswith("json"):
            text = text[4:]
    text = text.strip()

    data = json.loads(text)

    return ProblemResponse(
        topic=request.topic,
        problems=data.get("problems", []),
        hints=data.get("hints", []),
        solutions=data.get("solutions", []),
        subject=request.subject,
        difficulty_level=request.difficulty_level
    )
