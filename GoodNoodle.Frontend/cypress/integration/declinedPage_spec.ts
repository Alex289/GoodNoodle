describe('Declined page tests', () => {
  it('Should create a user', () => {
    cy.visit('/register');

    cy.get('#input-name').type('Cypress Declined');
    cy.get('#input-email').type('cypress@declined.com');
    cy.get('#input-password').type('Cypress1#');
    cy.get('#input-re-password').type('Cypress1#');
    cy.get('.custom-checkbox').click();
    cy.get('button[status="primary"]').contains('Register').click();
  });

  it('Should make user status declined', () => {
    cy.visit('/login');

    cy.get('#input-email').type('goodnoodle.noreply@gmail.com');
    cy.get('#input-password').type('!Password123#');
    cy.get('button[status="primary"]').contains('Log In').click();

    cy.get('.user-name').contains('Admin').should('be.visible');
    cy.visit('/admin');

    cy.get('#edit-cypressdeclinedcom').click();
    cy.get('#user-status').click();
    cy.get('nb-option[value="2"]').click();
    cy.get('#save-user').click();
  });

  it('Should login as declined user', () => {
    cy.visit('/login');

    cy.get('#input-email').type('cypress@declined.com');
    cy.get('#input-password').type('Cypress1#');
    cy.get('button[status="primary"]').contains('Log In').click();
    cy.location('pathname').should('eq', '/declined');
  });
});
