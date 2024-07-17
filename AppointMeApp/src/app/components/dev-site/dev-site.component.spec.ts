import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevSiteComponent } from './dev-site.component';

describe('DevSiteComponent', () => {
  let component: DevSiteComponent;
  let fixture: ComponentFixture<DevSiteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DevSiteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DevSiteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
