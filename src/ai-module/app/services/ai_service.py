import os
import json
import google.generativeai as genai
from app.schemas.ai_schemas import (
    ExplanationRequest, ExplanationResponse,
    HintRequest, HintResponse,
    ProblemRequest, ProblemResponse,
    LessonChatRequest, LessonChatResponse,
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


async def lesson_chat(request: LessonChatRequest) -> LessonChatResponse:
    model = get_gemini_client()

    subject_name = SUBJECT_NAMES.get(request.subject, "materie")
    difficulty_name = DIFFICULTY_NAMES.get(request.difficulty_level, "începător")

    if not model:
        return LessonChatResponse(
            answer=f"[DEMO] Răspuns demonstrativ pentru întrebarea '{request.question}' "
                   f"despre lecția '{request.lesson_title}'. Adaugă GEMINI_API_KEY pentru răspunsuri reale."
        )

    lesson_content = request.lesson_content.strip() if request.lesson_content else "(lecția nu are conținut text disponibil)"

    system_prompt = f"""Ești un tutor AI prietenos și răbdător care ajută un elev să înțeleagă o lecție de {subject_name}.
Răspunzi în română, cu ton cald și încurajator, simplu și clar, adaptat unui nivel {difficulty_name}.
Folosește exemple concrete, pași mici și analogii potrivite vârstei școlare.
Bazează răspunsurile STRICT pe lecția de mai jos. Dacă întrebarea iese complet din contextul lecției,
spune politicos că nu poți răspunde la asta și redirecționează elevul către conținutul lecției sau către profesor.
Nu inventa informații care nu există în lecție. Nu da răspunsuri foarte lungi - max 4-5 paragrafe.
Nu folosi markdown complicat, doar text simplu cu liste numerotate sau cu liniuțe când e util.

=== LECȚIA: {request.lesson_title} (nivel {difficulty_name}) ===
{lesson_content}
=== SFÂRȘIT LECȚIE ===
"""

    # Construim conversația: trimitem system prompt ca primul mesaj user, apoi history, apoi întrebarea curentă
    chat_history = []
    chat_history.append({"role": "user", "parts": [system_prompt]})
    chat_history.append({"role": "model", "parts": ["Am înțeles. Sunt aici să te ajut cu lecția. Ce întrebare ai?"]})

    for msg in request.history:
        role = "user" if msg.role == "user" else "model"
        chat_history.append({"role": role, "parts": [msg.content]})

    chat = model.start_chat(history=chat_history)
    response = chat.send_message(request.question)

    return LessonChatResponse(answer=response.text.strip())
