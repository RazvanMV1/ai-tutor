from fastapi import APIRouter, HTTPException
from app.schemas.ai_schemas import LessonChatRequest, LessonChatResponse
from app.services.ai_service import lesson_chat
import traceback
import logging

logger = logging.getLogger("uvicorn.error")
router = APIRouter()


@router.post("", response_model=LessonChatResponse)
async def create_lesson_chat(request: LessonChatRequest):
    try:
        return await lesson_chat(request)
    except Exception as e:
        tb = traceback.format_exc()
        logger.error(f"Lesson chat error: {e}\n{tb}")
        raise HTTPException(status_code=500, detail=f"{type(e).__name__}: {str(e)}")
