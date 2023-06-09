DROP DATABASE finances;

CREATE DATABASE finances;

CREATE TABLE finances.tb_user (
    id BIGINT NOT NULL AUTO_INCREMENT,
    name VARCHAR(60) NOT NULL,
    lastName VARCHAR(60) NOT NULL,
    email VARCHAR(100) NOT NULL,
    password VARCHAR(128) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    photo LONGBLOB,
    PRIMARY KEY (id),
    UNIQUE(email)
);

CREATE TABLE finances.tb_category (
    id BIGINT NOT NULL AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(255) NOT NULL,
    type VARCHAR(1) DEFAULT 'D',
    PRIMARY KEY (id)
);

CREATE TABLE finances.tb_transaction (
    id BIGINT NOT NULL AUTO_INCREMENT,
    description VARCHAR(255) NOT NULL,
    value DOUBLE NOT NULL,
    type VARCHAR(1) DEFAULT 'D',
    date DATE NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    fk_user BIGINT NOT NULL,
    fk_category BIGINT NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_transactionsXuser
        FOREIGN KEY (fk_user) REFERENCES tb_user (id)
        ON DELETE CASCADE,
    CONSTRAINT fk_transactionXCategory
        FOREIGN KEY (fk_category) REFERENCES tb_category (id)
);

/* Add 3 Users tb_user */
INSERT INTO finances.tb_user (name, lastName, email,  password)
    VALUES ('Olivia Johnson', 'Lee', 'olivia.lee@gmail.com', '47b5a8dd9249434323c32894204e87ef6b530d49ea124c5142fd5086e67d1b8ea2d2a4b342ef6eb473e540e2ea5a5c73a111d3f9696344ae5bd4b15292fce6d5');
    /*VALUES ('Olivia Johnson', 'Lee', 'olivia.lee@gmail.com', 'f9G@x2#s');*/
INSERT INTO finances.tb_user (name, lastName, email,  password)
    VALUES ('Ethan Wilson', 'Wong', 'ethan.wong@hotmail.com', '78b55b0da4100fb1be2a95621caf8926357ff2d20ccebdd6e3e50802acb0d88534344ea15031b8291b13012a9336089acc15ca4d817b124208b125850f92ca89');
    /*VALUES ('Ethan Wilson', 'Wong', 'ethan.wong@hotmail.com', 'D3v$P7@t');*/
INSERT INTO finances.tb_user (name, lastName, email,  password)
    VALUES ('Ava Chen', 'Patel', 'ava.patel@yahoo.com', 'e9420933792e91af4204297c630bacdaa64298469c7591a0cd82b9a54d66a8f8becb11e7ed380173a375e35958792e658731bd23e8396d01eacf3b9bfffa454f');
    /*VALUES ('Ava Chen', 'Patel', 'ava.patel@yahoo.com', 'B1r#K9%p');*/

/* Add 11 Expenses tb_category */
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Housing', 'Rent/mortgage payments, utilities, repairs, and maintenance', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Transportation', 'Car payments, gas, maintenance, and public transportation', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Food', 'Groceries and dining out expenses', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Entertainment', 'Expenses for movies, concerts, and other leisure activities', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Health', 'Expenses for health insurance, doctor visits, and medication', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Clothing', 'Expenses for clothing, shoes, and accessories', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Education', 'Expenses for tuition, books, and other education-related costs', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Debt', 'Payments for loans, credit cards, and other debt', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Taxes', 'Income taxes, property taxes, and other taxes', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Travel', 'Expenses for flights, hotels, and other travel-related costs', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Others', 'Expenses that do not fit into any other category', 'D');

/* Add 7 Earnings tb_category */
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Personal Income', 'Salary, commissions, bonuses, awards, tips, and other forms of compensation that an individual receives from their work.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Business Income', 'Net profit, revenue, return on investment (ROI), and other financial metrics related to a company’s performance.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Investment Income', 'Interest, dividends, capital gains, and other forms of income generated from financial investments.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Passive Income', 'Rental income, royalties from intellectual property, online advertising income, and other forms of income generated without the need for active work.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Gambling/Lottery Winnings', 'Winnings from gambling and lotteries.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Other', 'Income that does not fit into any of the categories.', 'R');

/* Add 7 Expenses Transactions tb_transaction */
INSERT INTO finances.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Electric Bill', 5.0, '2023-04-10', 1, 3);
INSERT INTO finances.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Groceries', 50.0, '2023-04-12', 2, 4);
INSERT INTO finances.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Gasoline', 30.0, '2023-04-04', 3, 5);
INSERT INTO finances.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Internet Bill', 50.0, '2023-04-01', 2, 3);
INSERT INTO finances.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Dinner at Restaurant', 80.0, '2023-04-06', 1, 6);
INSERT INTO finances.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Haircut', 25.0, '2023-04-13', 3, 1);
INSERT INTO finances.tb_transaction (description, value, date, fk_user, fk_category)
    VALUES ('Movie Ticket', 12.0, '2023-04-19', 2, 7);

/* Add 3 Income Transactions tb_transaction */
INSERT INTO finances.tb_transaction (description, value, type, date, fk_user, fk_category)
    VALUES ('Salary', 5000.0, 'R', '2023-04-07', 1, 12);
INSERT INTO finances.tb_transaction (description, value, type, date, fk_user, fk_category)
    VALUES ('Rental Income', 800.0, 'R', '2023-04-15', 2, 15);
INSERT INTO finances.tb_transaction (description, value, type, date, fk_user, fk_category)
    VALUES ('Investment Dividend', 150.0, 'R', '2023-03-05', 3, 14);

/*
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Moradia', 'Aluguel, hipoteca, condomínio, serviços públicos, reparos e manutenção da casa.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Transporte', 'Combustível, manutenção do veículo, seguro, impostos, transporte público.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Alimentação', 'Mercearia, restaurantes, cafeterias, lanches.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Entretenimento', 'Cinema, teatro, shows, esportes, hobbies.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Saúde', 'Seguro saúde, consultas médicas, medicamentos, tratamentos.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Vestuário', 'Roupas, sapatos, acessórios.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Educação', 'Mensalidades escolares, livros, cursos, treinamentos.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Dívidas', 'Pagamentos de empréstimos, cartões de crédito, juros.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Impostos', 'Imposto de renda, IPTU, IPVA, taxas.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Viagem', 'Passagens aéreas, hospedagem, transporte, alimentação.', 'D');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Outros', 'Gastos que não se encaixam em nenhuma outra categoria.', 'D');
*/

/*
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Ganhos Pessoais', 'Salário, comissões, bônus, prêmios, gorjetas e outras formas de remuneração que uma pessoa recebe em seu trabalho.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Ganhos Empresariais', 'Lucro líquido, receita, retorno sobre o investimento (ROI) e outras métricas financeiras relacionadas ao desempenho da empresa.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Ganhos de Investimento', 'Juros, dividendos, ganhos de capital e outras formas de renda geradas a partir de investimentos financeiros.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Ganhos Passivos', 'Renda de aluguel, royalties de propriedade intelectual, renda de publicidade online e outras formas de renda geradas sem a necessidade de trabalho ativo.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Ganhos de Jogo/Loteria', 'Ganhos provenientes de jogos de azar e loterias.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Outras Fontes de Renda', 'Heranças, doações, vendas de bens pessoais e outras formas de renda que não se encaixam em outras categorias.', 'R');
INSERT INTO finances.tb_category (name, description, type)
    VALUES ('Outros', 'Receitas que não se enquadram em nenhuma das categorias.', 'R');
*/