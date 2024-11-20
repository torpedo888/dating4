import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Question {
  id: number;
  text: string;
  correctOptionId: number;
  quizId: number;
  categoryId: number;
}

export interface Option {
  id: number;
  text: string;
  questionId: number;
}

export interface QuestionResponse {
  question: Question;
  options: Option[];
}

@Injectable({
  providedIn: 'root',
})
export class QuestionService {
  private apiUrl = 'http://localhost:5233/api/questions/create';

  private apiGetUrl = 'http://localhost:5233/api/questions';

  constructor(private http: HttpClient) {}

  saveQuestion(question: Question): Observable<Question> {
    return this.http.post<Question>(this.apiUrl, question);
  }

  getQuestions(): Observable<QuestionResponse[]> {
    return this.http.get<QuestionResponse[]>(this.apiGetUrl);
  }
}
