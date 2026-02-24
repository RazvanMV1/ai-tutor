from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.routers import explanations, problems, health

app = FastAPI(
    title="AiTutor AI Module",
    description="AI-powered explanations and problem generation for students",
    version="1.0.0"
)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(health.router, prefix="/health", tags=["Health"])
app.include_router(explanations.router, prefix="/api/explanations", tags=["Explanations"])
app.include_router(problems.router, prefix="/api/problems", tags=["Problems"])


@app.get("/")
async def root():
    return {"message": "AiTutor AI Module is running!"}
