import { Component, OnInit } from '@angular/core';
import { QuestionService, QuestionResponse } from '../question.service'; // Import QuestionResponse from service
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css'],
  imports: [CommonModule],
})
export class QuestionListComponent implements OnInit {
  questions: QuestionResponse[] = []; // Expect the structure directly as QuestionResponse[]

  constructor(private questionService: QuestionService) {}

  ngOnInit(): void {
    this.loadQuestions();
  }

  loadQuestions(): void {
    this.questionService.getQuestions().subscribe(
      (data: QuestionResponse[]) => {
        this.questions = data;

        console.log(
          'Loaded Questions:',
          JSON.stringify(this.questions, null, 2)
        );
      },
      (error) => {
        console.error('Error fetching questions', error);
      }
    );
  }

  getCorrectOptionText(item: QuestionResponse): string {
    const correctOption = item.options.find(
      (option) => option.id === item.question.correctOptionId
    );
    return correctOption ? correctOption.text : 'N/A';
  }
}
