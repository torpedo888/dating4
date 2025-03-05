import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Question } from '../_models/Question';
import { QuestionService } from '../question.service';
import { FormsModule } from '@angular/forms'; 


@Component({
  standalone: true,
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css'],
  imports: [CommonModule, FormsModule],
})
export class QuestionListComponent implements OnInit {
 
  questions: Question[] = [];
  selectedAnswers: { [questionId: number]: number } = {}; 
  errorMessage: string = '';

  // These are the properties to store quiz results
  correctAnswers: number = 0;
  score: number = 0;
  totalQuestions: number = 0;

  constructor(private questionService: QuestionService) {}

  ngOnInit() {
    this.questionService.getQuestions().subscribe({
      next: (data) => {
        console.log("data is coming:")
        console.log(data); // Log the data here
        this.questions = data;
      },
      error: (err) => console.error('Error fetching questions', err),
    });
  }

  submitAnswers() {
    if (this.questions.some(q => !this.selectedAnswers[q.id])) {
      this.errorMessage = 'Please answer all questions before submitting.';
      return;
    }
  
    const answers = Object.entries(this.selectedAnswers).map(([questionId, selectedOptionId]) => ({
      questionId: Number(questionId),
      selectedOptionId
    }));
  
    // this.questionService.submitAnswers(answers).subscribe({
    //   next: (result) => {
    //     console.log('Quiz result:', result);
    //     alert(`You got ${result.CorrectAnswers} out of ${result.TotalQuestions} correct! Score: ${result.Score}%`);
    //   },
    //   error: (err) => console.error('Error submitting answers', err)
    // });

      this.questionService.submitAnswers(answers).subscribe((response) => {
        this.correctAnswers = response.correctAnswers;
        this.score = response.score;
        this.totalQuestions = response.totalQuestions;
      });
    
  }
  
}
